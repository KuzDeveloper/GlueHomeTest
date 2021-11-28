using System;
using TT.Deliveries.Core.Enums;

namespace TT.Deliveries.Core.Entities
{
    public class Delivery
    {
        public Guid Id { get; set; }

        public DeliveryState State { get; set; }

        public AccessWindow AccessWindow { get; set; }

        public DeliveryRecipient Recipient { get; set; }

        public Guid OrderId { get; set; }

        public Order Order { get; set; }
    }
}
