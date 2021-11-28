using System;

namespace TT.Deliveries.Core.Interfaces
{
    public interface IUrlsProvider
    {
        string GetDeliveryUrl(Guid? id);

        string CancelDeliveryUrl(Guid? id);

        string UpdateDelivery(Guid? id);
    }
}
