using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.Core;
using S2Cognition.Integrations.Core.Models;
using S2Cognition.Integrations.Zoom.Core.Data;
using S2Cognition.Integrations.Zoom.Core.Models;
using S2Cognition.Integrations.Zoom.Phones.Data;

using System.Text.Json;

namespace S2Cognition.Integrations.Zoom.Phones;

public interface IZoomPhoneNativeClient : IIntegration<ZoomConfiguration>
{
    Task<ZoomGetCallQueuesResponse> GetZoomCallQueues(string accessToken, GetCallQueuesRequest req);
    Task<ZoomGetCallQueueMemberResponse> GetZoomCallQueueMembers(string accessToken, ZoomGetCallQueueMemberRequest request);
    Task<ZoomGetUsersPagedResponse> GetZoomUsers(string accessToken, GetUsersRequest req);
    Task<SetCallQueueMemberResponse> SetZoomCallQueueMembers(string accessToken, SetZoomCallQueueMemberRequest req);
    Task<RemoveCallQueueMemberResponse> RemoveZoomCallQueueMembers(string accessToken, RemoveZoomCallQueueMemberRequest req);
}

internal class ZoomPhoneNativeClient : Integration<ZoomConfiguration>, IZoomPhoneNativeClient
{
    internal ZoomPhoneNativeClient(IServiceProvider ioc)
        : base(ioc)
    {
    }

    public async Task<ZoomGetCallQueuesResponse> GetZoomCallQueues(string accessToken, GetCallQueuesRequest req)
    {
        var clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

        using var client = clientFactory.Create();
        client.SetAuthorization(accessToken, AuthorizationType.Bearer);

        var route = $"https://api.zoom.us/v2/phone/call_queues";
        var zoomData = await client.Get<ZoomGetCallQueuesResponse>(route);


        return JsonSerializer.Deserialize<ZoomGetCallQueuesResponse>(JsonSerializer.Serialize(zoomData))
            ?? throw new InvalidOperationException($"Cannot deserialize {nameof(ZoomGetCallQueuesResponse)}");
    }

    public async Task<ZoomGetCallQueueMemberResponse> GetZoomCallQueueMembers(string accessToken, ZoomGetCallQueueMemberRequest req)
    {
        var clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

        using var client = clientFactory.Create();
        client.SetAuthorization(accessToken, AuthorizationType.Bearer);

        var route = $"https://api.zoom.us/v2/phone/call_queues/{req.CallQueueId}/members";
        var zoomData = await client.Get<ZoomGetCallQueueMemberResponse>(route);

        return JsonSerializer.Deserialize<ZoomGetCallQueueMemberResponse>(JsonSerializer.Serialize(zoomData))
            ?? throw new InvalidOperationException($"Cannot deserialize {nameof(ZoomGetCallQueueMemberResponse)}");
    }

    public async Task<ZoomGetUsersPagedResponse> GetZoomUsers(string accessToken, GetUsersRequest req)
    {
        var clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

        using var client = clientFactory.Create();
        client.SetAuthorization(accessToken, AuthorizationType.Bearer);

        var route = $"https://api.zoom.us/v2/phone/users?status=activate";
        var zoomData = await client.Get<ZoomGetUsersPagedResponse>(route);

        return zoomData ?? new ZoomGetUsersPagedResponse();
    }

    public async Task<SetCallQueueMemberResponse> SetZoomCallQueueMembers(string accessToken, SetZoomCallQueueMemberRequest req)
    {
        var clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

        using var client = clientFactory.Create();
        client.SetAuthorization(accessToken, AuthorizationType.Bearer);

        var route = $"https://api.zoom.us/v2/phone/call_queues/{req.CallQeuueId}/members";

        var queueMember = new ZoomSetCallQueueMemberRequest
        {
            Members = new Members
            {
                Users = new List<PhoneUser>
                {
                    new PhoneUser { Id = req.UserId, Email = req.UserEmail }
                }
            }
        };

        return await client.PostJsonObject<SetCallQueueMemberResponse>(route, queueMember) ?? new SetCallQueueMemberResponse();
    }

    public async Task<RemoveCallQueueMemberResponse> RemoveZoomCallQueueMembers(string accessToken, RemoveZoomCallQueueMemberRequest req)
    {
        var clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

        using var client = clientFactory.Create();
        client.SetAuthorization(accessToken, AuthorizationType.Bearer);

        var route = $"https://api.zoom.us/v2/phone/call_queues/{req.CallQeuueId}/members/{req.UserId}";

        return await client.Delete<RemoveCallQueueMemberResponse>(route) ?? new RemoveCallQueueMemberResponse();
    }



    //private async Task<IHttpClient>? GetZoomClient(string accessToken)
    //{
    //    var clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

    //    var client = clientFactory.Create();
    //    client.SetAuthorization(accessToken, AuthorizationType.Bearer);

    //    return Task.FromResult(client);
    //}
}
