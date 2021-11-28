using System;
using System.Collections.Generic;
using TT.Deliveries.Core.Enums;
using TT.Deliveries.Core.Interfaces;
using TT.Deliveries.Web.Api.Interfaces;

namespace TT.Deliveries.Web.Api.Builders
{
    public class ActionsBuilder : IActionsBuilder
    {
        private const string CancelAction = "cancel";
        private const string UpdateAction = "update";

        private readonly IUrlsProvider _urlsProvider;

        public ActionsBuilder(IUrlsProvider urlsProvider)
        {
            _urlsProvider = urlsProvider;
        }

        public IDictionary<string, string> GetActions(Guid id, DeliveryState deliveryState)
        {
            var actions = new Dictionary<string, string>();

            switch (deliveryState)
            {
                case DeliveryState.Created:
                    actions.Add(CancelAction, _urlsProvider.CancelDeliveryUrl(id));
                    actions.Add(UpdateAction, _urlsProvider.UpdateDelivery(id));
                    break;
                case DeliveryState.Approved:
                    actions.Add(CancelAction, _urlsProvider.CancelDeliveryUrl(id));
                    actions.Add(UpdateAction, _urlsProvider.UpdateDelivery(id));
                    break;
                case DeliveryState.Completed:
                case DeliveryState.Cancelled:
                case DeliveryState.Expired:
                    actions = null;
                    break;
            }

            return actions;
        }
    }
}
