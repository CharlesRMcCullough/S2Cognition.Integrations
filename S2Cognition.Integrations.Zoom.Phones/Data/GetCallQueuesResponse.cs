using System.Text.Json.Serialization;

namespace S2Cognition.Integrations.Zoom.Phones.Data
{
    public class GetCallQueueResponse
    {
        public GetCallQueueResponse()
        {
        }

        [JsonPropertyName("call_queues")]
        public List<CallQueueRecord>? CallQueues { get; set; }
        [JsonPropertyName("next_page_token")]
        public string? NextPageToken { get; set; }
        [JsonPropertyName("page_size")]
        public int? PageSize { get; set; }
        [JsonPropertyName("total_records")]
        public int? TotalRecords { get; set; }
    }

    public class CallQueueRecord
    {
        public CallQueueRecord()
        {
        }

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

    public class PhoneNumberRecord
    {
        public PhoneNumberRecord()
        {
        }

        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("number")]
        public string? Number { get; set; }
        [JsonPropertyName("source")]
        public string? Source { get; set; }
    }

    public class SiteRecord
    {
        public SiteRecord()
        {
        }

        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }


}

//public class CallQueue
//{
//    public string extension_id { get; set; }
//    public long extension_number { get; set; }
//    public string id { get; set; }
//    public string name { get; set; }
//    public List<PhoneNumber> phone_numbers { get; set; }
//    public Site site { get; set; }
//    public string status { get; set; }
//}

//public class PhoneNumber
//{
//    public string id { get; set; }
//    public string number { get; set; }
//    public string source { get; set; }
//}

//public class Root
//{
//    public List<CallQueue> call_queues { get; set; }
//    public string next_page_token { get; set; }
//    public int page_size { get; set; }
//    public int total_records { get; set; }
//}

//public class Site
//{
//    public string id { get; set; }
//    public string name { get; set; }
//}

