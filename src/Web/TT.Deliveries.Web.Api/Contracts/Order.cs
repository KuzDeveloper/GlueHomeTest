using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace TT.Deliveries.Web.Api.Contracts
{
    public class Order
    {
        [DataMember]
        [JsonProperty("id")]
        public Guid Id { get; set; } // This could also be a "Self" resource url field when the test implementation requires it.

        [DataMember]
        [JsonProperty("orderNumber")]
        public string OrderNumber { get; set; }

        [DataMember]
        [JsonProperty("sender")]
        public string Sender { get; set; }
    }
}
