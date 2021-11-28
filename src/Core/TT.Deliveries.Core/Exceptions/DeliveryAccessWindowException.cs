using System;

namespace TT.Deliveries.Core.Exceptions
{
    public class DeliveryAccessWindowException : Exception
    {
        public DeliveryAccessWindowException(string message) : base(message)
        {
        }
    }
}
