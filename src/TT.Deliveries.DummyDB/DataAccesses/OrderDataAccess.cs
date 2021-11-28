using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TT.Deliveries.Core.Entities;
using TT.Deliveries.Core.Interfaces;

namespace TT.Deliveries.DummyDB.DataAccesses
{
    public class OrderDataAccess : IOrderDataAccess
    {
        private readonly IMapper _mapper;
        private readonly List<Entities.Order> _dummyOrderTable;

        public OrderDataAccess(IMapper mapper)
        {
            _mapper = mapper;
            _dummyOrderTable = new List<Entities.Order>();
        }

        public Task<Guid> AddOrderAsync(IDbConnection connection, IDbTransaction transaction, Order order)
        {
            var mappedOrder = _mapper.Map<Entities.Order>(order);

            mappedOrder.Id = Guid.NewGuid();

            _dummyOrderTable.Add(mappedOrder);

            return Task.FromResult(mappedOrder.Id);
        }

        public Task<Order> GetOrderAsync(IDbConnection connection, IDbTransaction transaction, Guid id)
        {
            var order = _dummyOrderTable.SingleOrDefault(o => o.Id == id);

            var mappedOrder = _mapper.Map<Order>(order);

            return Task.FromResult(mappedOrder);
        }
    }
}
