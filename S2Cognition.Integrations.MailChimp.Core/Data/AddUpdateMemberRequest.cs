namespace S2Cognition.Integrations.MailChimp.Core.Data
{
    public class AddUpdateMemberRequest
    {
        public string? ListId { get; set; }
        public string? EmailAddress { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

    }
}
