using System;

namespace TT.Deliveries.Core.Exceptions
{
    public class DeliveryNotFoundException : Exception
    {
        public DeliveryNotFoundException(string message) : base(message)
        {
        }
    }
}
