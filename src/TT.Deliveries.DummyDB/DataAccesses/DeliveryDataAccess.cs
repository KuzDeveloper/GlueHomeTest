using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TT.Deliveries.Core.Entities;
using TT.Deliveries.Core.Enums;
using TT.Deliveries.Core.Exceptions;
using TT.Deliveries.Core.Interfaces;

namespace TT.Deliveries.DummyDB.DataAccesses
{
    public class DeliveryDataAccess : IDeliveryDataAccess
    {
        private readonly IMapper _mapper;
        private readonly List<Entities.Delivery> _dummyDeliveryTable;

        public DeliveryDataAccess(IMapper mapper)
        {
            _mapper = mapper;
            _dummyDeliveryTable = new List<Entities.Delivery>();
        }

        public Task<Guid> AddDeliveryAsync(IDbConnection connection, IDbTransaction transaction, Delivery delivery)
        {
            var mappedDelivery = _mapper.Map<Entities.Delivery>(delivery);
            
            mappedDelivery.Id = Guid.NewGuid();

            _dummyDeliveryTable.Add(mappedDelivery);

            return Task.FromResult(mappedDelivery.Id);
        }

        public Task CancelDeliveryAsync(IDbConnection connection, IDbTransaction transaction, Guid id)
        {
            var delivery = _dummyDeliveryTable.SingleOrDefault(d => d.Id == id);

            if (delivery == null)
            {
                throw new DeliveryNotFoundException($"Delivery with id {id} was not found!");
            }

            delivery.State = (byte)Core.Enums.DeliveryState.Cancelled;

            return Task.CompletedTask;
        }
                
        public Task<Delivery> GetDeliveryAsync(IDbConnection connection, IDbTransaction transaction, Guid id)
        {
            var delivery = _dummyDeliveryTable.SingleOrDefault(d => d.Id == id);

            var mappedResult = _mapper.Map<Delivery>(delivery);

            return Task.FromResult(mappedResult);
        }

        public Task<IEnumerable<Delivery>> GetDeliveriesForExpirationAsync(IDbConnection connection, IDbTransaction transaction)
        {
            var deliveriesForExpiration = _dummyDeliveryTable.Where(d => d.State <= (byte)DeliveryState.Approved);

            var mappedDeliveries = _mapper.Map<IEnumerable<Delivery>>(deliveriesForExpiration);

            return Task.FromResult(mappedDeliveries);
        }

        public Task UpdateDeliveryAsync(IDbConnection connection, IDbTransaction transaction, Guid id, Delivery delivery)
        {
            var existingDelivery = _dummyDeliveryTable.SingleOrDefault(d => d.Id == id);

            if (delivery == null)
            {
                throw new DeliveryNotFoundException($"Delivery with id {id} was not found!");
            }

            existingDelivery.AccessWindowDetails = delivery.AccessWindow.ToJson();
            existingDelivery.RecipientDetails = delivery.Recipient.ToJson();
            existingDelivery.State = (byte)delivery.State;

            return Task.CompletedTask;
        }
    }
}
