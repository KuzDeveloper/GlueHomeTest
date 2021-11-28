using System;
using System.Data;
using System.Threading.Tasks;
using TT.Deliveries.Core.Entities;

namespace TT.Deliveries.Core.Interfaces
{
    public interface IOrderDataAccess
    {
        Task<Order> GetOrderAsync(IDbConnection connection, IDbTransaction transaction, Guid orderId);

        Task<Guid> AddOrderAsync(IDbConnection connection, IDbTransaction transaction, Order order);
    }
}
