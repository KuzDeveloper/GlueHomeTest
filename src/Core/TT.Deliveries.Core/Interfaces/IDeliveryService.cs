using System;
using System.Threading.Tasks;
using TT.Deliveries.Core.Entities;

namespace TT.Deliveries.Core.Interfaces
{
    public interface IDeliveryService
    {
        Task<Delivery> GetDeliveryAsync(Guid id);

        Task<Guid> AddDeliveryAsync(Delivery delivery);

        Task UpdateDeliveryAsync(Guid id, Delivery delivery);

        Task CancelDeliveryAsync(Guid id);

        Task SetStatesAsPerAccessWindowAsync();
    }
}
