using SharedCodeLibrary.Interfaces;
using TT.Deliveries.Core.Entities;
using TT.Deliveries.Core.Enums;
using TT.Deliveries.Core.Exceptions;
using TT.Deliveries.Core.Interfaces;

namespace TT.Deliveries.Core.Validators
{
    public class DeliveryValidator : IDeliveryValidator
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeliveryValidator(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public void ValidateForCreation(Delivery delivery)
        {
            ValidateForNull(delivery);
            ValidateAccessWindow(delivery);
        }

        public void ValidateForUpdating(Delivery delivery)
        {
            ValidateForNull(delivery);
            ValidateAccessWindow(delivery);

            if ((byte)delivery.State > (byte)DeliveryState.Approved)
            {
                throw new DeliveryStatusException($"Deliveries can only be updated while in state {DeliveryState.Created} and {DeliveryState.Approved}! Current state is: {delivery.State}.");
            }
        }

        public void ValidateForCancellation(Delivery delivery)
        {
            ValidateForNull(delivery);
            ValidateAccessWindow(delivery);

            if ((byte)delivery.State > (byte)DeliveryState.Approved)
            {
                throw new DeliveryStatusException($"Deliveries can only be cancelled while in state {DeliveryState.Created} and {DeliveryState.Approved}! Current state is: {delivery.State}.");
            }
        }

        private void ValidateForNull(Delivery delivery)
        {
            if (delivery == null)
            {
                throw new DeliveryNullException($"Delivery was not specified!");
            }
        }

        private void ValidateAccessWindow(Delivery delivery)
        {
            var currentDate = _dateTimeProvider.GetCurrentTimeUTC();

            if (delivery.AccessWindow.StartTime >= currentDate ||
                delivery.AccessWindow.EndTime <= currentDate)
            {
                var startDateString = _dateTimeProvider.ConvertToDateString(delivery.AccessWindow.StartTime);
                var endDateString = _dateTimeProvider.ConvertToDateString(delivery.AccessWindow.EndTime);

                throw new DeliveryAccessWindowException($"Current date is not within the delivery's access window ({startDateString}-{endDateString}).");
            }
        }
    }
}
