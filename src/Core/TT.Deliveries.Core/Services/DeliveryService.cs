using SharedCodeLibrary.Interfaces;
using SharedCodeLibrary.Services;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TT.Deliveries.Core.Entities;
using TT.Deliveries.Core.Exceptions;
using TT.Deliveries.Core.Interfaces;

namespace TT.Deliveries.Core.Services
{
    public class DeliveryService : DbOperatingService, IDeliveryService
    {
        private readonly IDeliveryDataAccess _deliveryDataAccess;
        private readonly IOrderDataAccess _orderDataAccess;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IDeliveryValidator _deliveryValidator;

        public DeliveryService(IDbConnectionProvider dbConnectionProvider,
            IDeliveryDataAccess deliveryDataAccess,
            IOrderDataAccess orderDataAccess,
            IDeliveryValidator deliveryValidator,
            IDateTimeProvider dateTimeProvider) : base(dbConnectionProvider)
        {
            _deliveryDataAccess = deliveryDataAccess;
            _orderDataAccess = orderDataAccess;
            _deliveryValidator = deliveryValidator;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task<Guid> AddDeliveryAsync(Delivery delivery)
        {
            return WithConnectionTransaction((connection, transaction) => AddDeliveryAsync(connection, transaction, delivery));
        }

        public Task CancelDeliveryAsync(Guid id)
        {
            return WithConnectionTransaction((connection, transaction) => CancelDeliveryAsync(connection, transaction, id));
        }

        public Task<Delivery> GetDeliveryAsync(Guid id)
        {
            return WithConnectionTransaction((connection, transaction) => GetDeliveryAsync(connection, transaction, id));
        }

        public Task SetStatesAsPerAccessWindowAsync()
        {
            return WithConnectionTransaction((connection, transaction) => SetStatesAsPerAccessWindowAsync(connection, transaction));
        }
        
        public Task UpdateDeliveryAsync(Guid id, Delivery delivery)
        {
            return WithConnectionTransaction((connection, transaction) => UpdateDeliveryAsync(connection, transaction, id, delivery));
        }

        private async Task<Guid> AddDeliveryAsync(IDbConnection connection, IDbTransaction transaction, Delivery delivery)
        {
            _deliveryValidator.ValidateForCreation(delivery);

            var newOrderId = await _orderDataAccess.AddOrderAsync(connection, transaction, delivery.Order);

            delivery.OrderId = newOrderId;
            delivery.State = Enums.DeliveryState.Created;

            var newDeliveryId = await _deliveryDataAccess.AddDeliveryAsync(connection, transaction, delivery);

            return newDeliveryId;
        }

        private async Task CancelDeliveryAsync(IDbConnection connection, IDbTransaction transaction, Guid id)
        {
            var existingDelivery = await _deliveryDataAccess.GetDeliveryAsync(connection, transaction, id);

            _deliveryValidator.ValidateForCancellation(existingDelivery);

            await _deliveryDataAccess.CancelDeliveryAsync(connection, transaction, id);
        }

        private async Task<Delivery> GetDeliveryAsync(IDbConnection connection, IDbTransaction transaction, Guid id)
        {
            var delivery = await _deliveryDataAccess.GetDeliveryAsync(connection, transaction, id);

            if (delivery == null)
            {
                throw new DeliveryNotFoundException($"Delivery with id {id} was not found!");
            }

            var order = await _orderDataAccess.GetOrderAsync(connection, transaction, delivery.OrderId);

            delivery.Order = order;

            return delivery;
        }

        private async Task UpdateDeliveryAsync(IDbConnection connection, IDbTransaction transaction, Guid id, Delivery delivery)
        {
            _deliveryValidator.ValidateForUpdating(delivery);

            var existingDelivery = await GetDeliveryAsync(connection, transaction, id);

            existingDelivery.AccessWindow = delivery.AccessWindow;
            existingDelivery.Recipient = delivery.Recipient;
            existingDelivery.State = delivery.State;

            await _deliveryDataAccess.UpdateDeliveryAsync(connection, transaction, id, existingDelivery);
        }

        private async Task SetStatesAsPerAccessWindowAsync(IDbConnection connection, IDbTransaction transaction)
        {
            var deliveriesForExpiration = await _deliveryDataAccess.GetDeliveriesForExpirationAsync(connection, transaction);

            var currentTime = _dateTimeProvider.GetCurrentTimeUTC();

            var expiredDeliveries = deliveriesForExpiration.Where(d => d.AccessWindow.EndTime < currentTime);

            expiredDeliveries?.ToList().ForEach(d =>
            {
                d.State = Enums.DeliveryState.Expired;

                _deliveryDataAccess.UpdateDeliveryAsync(connection, transaction, d.Id, d);
            });
        }
    }
}
