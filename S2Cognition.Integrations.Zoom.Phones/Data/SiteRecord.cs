using System.Text.Json.Serialization;

namespace S2Cognition.Integrations.Zoom.Phones.Data;
public class SiteRecord
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

