using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TT.Deliveries.Core.Entities;

namespace TT.Deliveries.Core.Interfaces
{
    public interface IDeliveryDataAccess
    {
        Task<Delivery> GetDeliveryAsync(IDbConnection connection, IDbTransaction transaction, Guid id);

        Task<IEnumerable<Delivery>> GetDeliveriesForExpirationAsync(IDbConnection connection, IDbTransaction transaction);

        Task<Guid> AddDeliveryAsync(IDbConnection connection, IDbTransaction transaction, Delivery delivery);

        Task UpdateDeliveryAsync(IDbConnection connection, IDbTransaction transaction, Guid id, Delivery delivery);

        Task CancelDeliveryAsync(IDbConnection connection, IDbTransaction transaction, Guid id);
    }
}
