using Newtonsoft.Json;

namespace TT.Deliveries.Core.Entities
{
    public class OrderDetails
    {
        public string OrderNumber { get; set; }

        public string Sender { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        public static OrderDetails FromJson(string json)
        {
            return string.IsNullOrWhiteSpace(json)
                ? null
                : JsonConvert.DeserializeObject<OrderDetails>(json);
        }
    }
}
