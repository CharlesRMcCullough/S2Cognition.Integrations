using System.Text.Json.Serialization;

namespace S2Cognition.Integrations.Zoom.Phones.Data;

public class CallQueueRecord
{
    [JsonPropertyName("extension_id")]
    public string? Extension_Id { get; set; }
    [JsonPropertyName("extension_number")]
    public long? ExtensionNumber { get; set; }
    [JsonPropertyName("id")]
    public string? id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("phone_numbers")]
    public IList<PhoneNumberRecord>? PhoneNumber { get; set; }
    [JsonPropertyName("site")]
    public SiteRecord? Site { get; set; }
    [JsonPropertyName("status")]
    public string? Status { get; set; }
}
