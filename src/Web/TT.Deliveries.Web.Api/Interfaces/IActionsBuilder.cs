using System;
using System.Collections.Generic;
using TT.Deliveries.Core.Enums;

namespace TT.Deliveries.Web.Api.Interfaces
{
    public interface IActionsBuilder
    {
        IDictionary<string, string> GetActions(Guid id, DeliveryState deliveryState);
    }
}
