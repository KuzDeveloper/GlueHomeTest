using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace TT.Deliveries.Web.Api.Contracts
{
    public class DeliveryRecipient
    {
        [DataMember]
        [JsonProperty("name")]
        public string Name { get; set; }

        [DataMember]
        [JsonProperty("address")]
        public string Address { get; set; }

        [DataMember]
        [JsonProperty("email")]
        public string Email { get; set; }

        [DataMember]
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
