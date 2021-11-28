using TT.Deliveries.Core.Interfaces;

namespace TT.Deliveries.Core.Providers
{
    public class UrlProviderTemplates : IUrlProviderTemplates
    {
        public string GetDeliveryUrlTemplate { get; set; }

        public string UpdateDeliveryUrlTemplate { get; set; }

        public string CancelDeliveryUrlTemplate { get; set; }
    }
}
