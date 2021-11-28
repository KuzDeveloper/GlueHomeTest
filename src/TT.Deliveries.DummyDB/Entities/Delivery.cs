using System;

namespace TT.Deliveries.DummyDB.Entities
{
    public class Delivery
    {
        public Guid Id { get; set; }

        public byte State { get; set; }

        public string AccessWindowDetails { get; set; }

        public string RecipientDetails { get; set; }

        public Guid OrderId { get; set; }
    }
}
