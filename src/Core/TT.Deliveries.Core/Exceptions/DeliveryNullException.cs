using System;

namespace TT.Deliveries.Core.Exceptions
{
    public class DeliveryNullException : Exception
    {
        public DeliveryNullException(string message) : base(message)
        {
        }
    }
}
