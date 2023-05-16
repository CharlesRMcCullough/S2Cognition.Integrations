using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.Core;
using S2Cognition.Integrations.MailChimp.Core.Data;
using S2Cognition.Integrations.MailChimp.Core.Models;

namespace S2Cognition.Integrations.MailChimp.Core;
public interface IMailChimpIntegration : IIntegration<MailChimpConfiguration>
{
    Task<AddUpdateMemberResponse> GetLists();
    Task<AddUpdateMemberResponse> MailChimpAddUpdateMember(AddUpdateMemberRequest req);
}
public class MailChimpIntegration : Integration<MailChimpConfiguration>, IMailChimpIntegration
{
    internal MailChimpIntegration(IServiceProvider ioc)
    : base(ioc)
    {
    }

    public override async Task Initialize(MailChimpConfiguration configuration)
    {
        await base.Initialize(configuration);

        var client = _serviceProvider.GetRequiredService<IMailChimpNativeClient>();
        await client.Initialize(new MailChimpConfiguration(_serviceProvider)
        {
            AccountId = configuration.AccountId,
            ClientId = configuration.ClientId,
            ClientSecret = configuration.ClientSecret
        });
    }

    public async Task<AddUpdateMemberResponse> MailChimpAddUpdateMember(AddUpdateMemberRequest req)
    {
        var client = _serviceProvider.GetRequiredService<IMailChimpNativeClient>();

        var response = client.MemberAddOrUpdate(req);

        return new AddUpdateMemberResponse();
    }

    public async Task<AddUpdateMemberResponse> GetLists()
    {
        var client = _serviceProvider.GetRequiredService<IMailChimpNativeClient>();

        var response = await client.GetLists();

        return new AddUpdateMemberResponse();
    }
}
