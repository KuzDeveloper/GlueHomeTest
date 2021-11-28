using TT.Deliveries.Core.Entities;

namespace TT.Deliveries.Core.Interfaces
{
    public interface IDeliveryValidator
    {
        void ValidateForCreation(Delivery delivery);

        void ValidateForUpdating(Delivery delivery);

        void ValidateForCancellation(Delivery delivery);
    }
}
