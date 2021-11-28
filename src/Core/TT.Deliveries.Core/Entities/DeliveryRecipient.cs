using Newtonsoft.Json;

namespace TT.Deliveries.Core.Entities
{
    public class DeliveryRecipient
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        public static DeliveryRecipient FromJson(string json)
        {
            return string.IsNullOrWhiteSpace(json)
                ? null
                : JsonConvert.DeserializeObject<DeliveryRecipient>(json);
        }
    }
}
