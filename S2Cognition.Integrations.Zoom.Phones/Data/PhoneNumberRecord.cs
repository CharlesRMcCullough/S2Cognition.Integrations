using System.Text.Json.Serialization;

namespace S2Cognition.Integrations.Zoom.Phones.Data
{
    public class PhoneNumberRecord
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("number")]
        public string? Number { get; set; }
        [JsonPropertyName("source")]
        public string? Source { get; set; }
    }
}
