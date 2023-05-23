using MailChimp.Net;
using MailChimp.Net.Models;
using S2Cognition.Integrations.Core;
using S2Cognition.Integrations.MailChimp.Core.Data;

namespace S2Cognition.Integrations.MailChimp.Core.Models;

public interface IMailChimpNativeClient : IIntegration<MailChimpConfiguration>
{
    Task<Member> MemberAddOrUpdate(string listId, Member member);
    Task<IEnumerable<List>> GetLists();
}
internal class MailChimpNativeClient : Integration<MailChimpConfiguration>, IMailChimpNativeClient
{
    internal MailChimpNativeClient(IServiceProvider ioc)
    : base(ioc)
    {
    }

    public async Task<Member> MemberAddOrUpdate(string listId, Member member)
    {
        var response = await new MailChimpManager(Configuration.AccountId).Members.AddOrUpdateAsync(listId, member);

        return response;
    }

    public async Task<IEnumerable<List>> GetLists()
    {
        var response = await new MailChimpManager(Configuration.AccountId).Lists.GetAllAsync();

        return response;
    }
}
