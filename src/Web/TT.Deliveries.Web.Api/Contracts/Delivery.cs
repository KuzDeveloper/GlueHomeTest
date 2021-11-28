using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TT.Deliveries.Web.Api.Contracts
{
    public class Delivery
    {
        [DataMember]
        [JsonProperty("self")]
        public string Self { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "actions", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, string> Actions { get; set; }

        [DataMember]
        [JsonProperty("state")]
        public string State { get; set; }

        [DataMember]
        [JsonProperty("accessWindow")]
        public AccessWindow AccessWindow { get; set; }

        [DataMember]
        [JsonProperty("recipient")]
        public DeliveryRecipient Recipient { get; set; }

        [DataMember]
        [JsonProperty("order")]
        public Order Order { get; set; }
    }
}
