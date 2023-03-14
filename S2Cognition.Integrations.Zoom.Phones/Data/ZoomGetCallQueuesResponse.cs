using System.Text.Json.Serialization;

namespace S2Cognition.Integrations.Zoom.Phones.Data
{
    public class ZoomGetCallQueuesResponse
    {
        [JsonPropertyName("call_queues")]
        public List<CallQueueRecord>? CallQueues { get; set; }
        [JsonPropertyName("next_page_token")]
        public string? NextPageToken { get; set; }
        [JsonPropertyName("page_size")]
        public int? PageSize { get; set; }
        [JsonPropertyName("total_records")]
        public int? TotalRecords { get; set; }
    }
}

