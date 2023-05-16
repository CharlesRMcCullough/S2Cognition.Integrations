using MailChimp.Net;
using MailChimp.Net.Models;
using S2Cognition.Integrations.Core;
using S2Cognition.Integrations.MailChimp.Core.Data;


namespace S2Cognition.Integrations.MailChimp.Core.Models;

public interface IMailChimpNativeClient : IIntegration<MailChimpConfiguration>
{
    Task<Member> MemberAddOrUpdate(AddUpdateMemberRequest req);
    Task<bool> GetLists();
}
internal class MailChimpNativeClient : Integration<MailChimpConfiguration>, IMailChimpNativeClient
{
    internal MailChimpNativeClient(IServiceProvider ioc)
    : base(ioc)
    {
    }

    public async Task<Member> MemberAddOrUpdate(AddUpdateMemberRequest req)
    {
        var member = new Member
        {
            EmailAddress = req.EmailAddress,
            Status = Status.Subscribed
        };

        var response = await new MailChimpManager(Configuration.AccountId).Members.AddOrUpdateAsync(req.ListId, member);

        return response;
    }

    public async Task<IEnumerable<List>> GetLists()
    {
        var response = await new MailChimpManager(Configuration.AccountId).Lists.GetAllAsync();

        return response;
    }
}
