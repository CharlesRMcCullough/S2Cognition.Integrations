namespace S2Cognition.Integrations.Zoom.Phones.Data
{
    public class GetCallQueueMemberResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Level { get; set; }
        public bool ReceiveCall { get; set; }
        public string? ExtensionId { get; set; }
    }
}
