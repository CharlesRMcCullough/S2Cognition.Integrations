﻿using MailChimp.Net.Models;
using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.Core;
using S2Cognition.Integrations.MailChimp.Core.Data;
using S2Cognition.Integrations.MailChimp.Core.Models;

namespace S2Cognition.Integrations.MailChimp.Core;
public interface IMailChimpIntegration : IIntegration<MailChimpConfiguration>
{
    Task<GetListsResponse>? GetLists();
    Task<AddUpdateMemberResponse>? MailChimpAddUpdateMember(AddUpdateMemberRequest req);
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

    public async Task<AddUpdateMemberResponse>? MailChimpAddUpdateMember(AddUpdateMemberRequest req)
    {
        if (req.ListId == null)
            throw new ArgumentException(nameof(req.ListId));

        if (string.IsNullOrWhiteSpace(req.EmailAddress))
            throw new ArgumentException(nameof(req.EmailAddress));

        var client = _serviceProvider.GetRequiredService<IMailChimpNativeClient>();

        var response = await client.MemberAddOrUpdate(req.ListId, new Member
        {
            ListId = req.ListId,
            EmailAddress = req.EmailAddress,
            Status = req.Subscribed ? Status.Subscribed : Status.Unsubscribed,
            FullName = req.FirstName,
            LastChanged = req.LastName
        });

        if (response != null)
        {
            return new AddUpdateMemberResponse
            {
                ListId = response.ListId,
                EmailAddress = response.EmailAddress
            };
        }

        return new AddUpdateMemberResponse();
    }

    public async Task<GetListsResponse>? GetLists()
    {
        List<GetListResponseItem> getListResponseItems = new List<GetListResponseItem>();

        var client = _serviceProvider.GetRequiredService<IMailChimpNativeClient>();

        var response = await client.GetLists();

        getListResponseItems.AddRange(response
            .Select(_ => new GetListResponseItem
            {
                ListId = _.Id,
                ListName = _.Name
            }));

        return new GetListsResponse
        {
            GetListResponseItems = getListResponseItems
        };
    }
}