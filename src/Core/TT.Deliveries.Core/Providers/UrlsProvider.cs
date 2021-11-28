using System;
using System.Globalization;
using TT.Deliveries.Core.Interfaces;

namespace TT.Deliveries.Core.Providers
{
    public class UrlsProvider : IUrlsProvider
    {
        private readonly IUrlProviderTemplates _urlProviderTemplates;

        public UrlsProvider(IUrlProviderTemplates urlProviderTemplates)
        {
            _urlProviderTemplates = urlProviderTemplates;
        }

        public string CancelDeliveryUrl(Guid? id)
        {
            return BuildUrl(_urlProviderTemplates.CancelDeliveryUrlTemplate, id);
        }

        public string GetDeliveryUrl(Guid? id)
        {
            return BuildUrl(_urlProviderTemplates.GetDeliveryUrlTemplate, id);
        }

        public string UpdateDelivery(Guid? id)
        {
            return BuildUrl(_urlProviderTemplates.UpdateDeliveryUrlTemplate, id);
        }

        private string BuildUrl(string template, Guid? id)
        {
            if (!id.HasValue)
            {
                return null;
            }

            return string.Format(CultureInfo.InvariantCulture, template, id.Value.ToString());
        }
    }
}
