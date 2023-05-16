using MailChimp.Net.Models;
using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.Core;
using S2Cognition.Integrations.MailChimp.Core.Data;
using S2Cognition.Integrations.MailChimp.Core.Models;

namespace S2Cognition.Integrations.MailChimp.Core;
public interface IMailChimpIntegration : IIntegration<MailChimpConfiguration>
{
    Task<GetListsResponse> GetLists();
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

        var member = new Member
        {
            ListId = req.ListId,
            EmailAddress = req.EmailAddress,
            Status = Status.Subscribed,
            FullName = req.FirstName,
            LastChanged = req.LastName
        };

        var response = await client.MemberAddOrUpdate(req.ListId, member);

        return new AddUpdateMemberResponse();
    }

    public async Task<GetListsResponse> GetLists()
    {
        List<GetListResponseItem> getListResponseItems = new List<GetListResponseItem>();

        var client = _serviceProvider.GetRequiredService<IMailChimpNativeClient>();

        var response = await client.GetLists();

        foreach (var item in response)
        {
            var listItem = new GetListResponseItem()
            {
                ListId = item.Id,
                ListName = item.Name
            };

            getListResponseItems.Add(listItem);
        }

        return new GetListsResponse
        {
            GetListResponseItems = getListResponseItems
        };
    }
}
