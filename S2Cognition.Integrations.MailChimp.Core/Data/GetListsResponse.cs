namespace S2Cognition.Integrations.MailChimp.Core.Data
{
    public class GetListsResponse
    {
        public IList<GetListResponseItem>? GetListResponseItems { get; set; }
    }

    public class GetListResponseItem
    {
        public string? ListId { get; set; }
        public string? ListName { get; set; }
    }
}
