using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace TT.Deliveries.Web.Api.Contracts
{
    public class AccessWindow
    {
        [DataMember]
        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        [DataMember]
        [JsonProperty("endTime")]
        public string EndTime { get; set; }
    }
}
