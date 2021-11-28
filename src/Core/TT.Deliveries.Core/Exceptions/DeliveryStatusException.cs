using System;

namespace TT.Deliveries.Core.Exceptions
{
    public class DeliveryStatusException : Exception
    {
        public DeliveryStatusException(string message) : base(message)
        {
        }
    }
}
