using MailChimp.Net.Models;

namespace S2Cognition.Integrations.MailChimp.Core.Data
{
    public class AddUpdateMemberRequest
    {
        public string? ListId { get; set; }
        public Status? MemberStatus { get; set; }
        public string? EmailAddress { get; set; }

    }
}
