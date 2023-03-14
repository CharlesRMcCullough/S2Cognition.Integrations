using System.Text.Json.Serialization;

namespace S2Cognition.Integrations.Zoom.Phones.Data
{
    public class ZoomGetCallQueueMemberResponse
    {
        public ZoomGetCallQueueMemberResponse() { }

        [JsonPropertyName("call_queue_members")]
        public IList<CallQueueMember>? CallQueueMembers { get; set; }
        [JsonPropertyName("next_page_token")]
        public string? NextPageToken { get; set; }
        [JsonPropertyName("page_size")]
        public int? PageSize { get; set; }
        [JsonPropertyName("total_records")]
        public int? TotalRecords { get; set; }
    }


    public class CallQueueMember
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("level")]
        public string? Level { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("receive_call")]
        public bool ReceiveCall { get; set; }
        [JsonPropertyName("extension_id")]
        public string? ExtensionId { get; set; }
    }
}
