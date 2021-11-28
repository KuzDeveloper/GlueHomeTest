using Newtonsoft.Json;
using System;

namespace TT.Deliveries.Core.Entities
{
    public class AccessWindow
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        public static AccessWindow FromJson(string json)
        {
            return string.IsNullOrWhiteSpace(json)
                ? null
                : JsonConvert.DeserializeObject<AccessWindow>(json);
        }
    }
}
