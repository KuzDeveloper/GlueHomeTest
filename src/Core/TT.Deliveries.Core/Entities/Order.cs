using System;

namespace TT.Deliveries.Core.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public OrderDetails OrderDetails { get; set; }
    }
}
