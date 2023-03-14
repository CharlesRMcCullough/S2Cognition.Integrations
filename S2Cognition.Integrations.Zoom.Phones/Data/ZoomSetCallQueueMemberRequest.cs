using System.Text.Json.Serialization;

namespace S2Cognition.Integrations.Zoom.Phones.Data
{
    public class ZoomSetCallQueueMemberRequest
    {
        [JsonPropertyName("members")]
        public Members? Members { get; set; }
    }

    public class Members
    {
        [JsonPropertyName("common_area_phone_ids")]
        public List<string>? CommonAreaPhoneIds { get; set; }
        [JsonPropertyName("common_area_ids")]
        public List<string>? CommonAreaIds { get; set; }
        [JsonPropertyName("users")]
        public List<PhoneUser>? Users { get; set; }
    }

    public class PhoneUser
    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }
}
