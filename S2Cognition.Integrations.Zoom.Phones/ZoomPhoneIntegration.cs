using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.Core;
using S2Cognition.Integrations.Core.Models;
using S2Cognition.Integrations.Zoom.Core.Data;
using S2Cognition.Integrations.Zoom.Core.Models;
using S2Cognition.Integrations.Zoom.Phones.Data;
using S2Cognition.Integrations.Zoom.Phones.Models;
using System.Text.Json;

namespace S2Cognition.Integrations.Zoom.Phones
{
    public interface IZoomPhoneIntegration : IIntegration<ZoomConfiguration>
    {
        Task<GetCallQueueResponse> GetCallQueues(GetUsersRequest request);
        Task<GetCallQueueMemberResponse> GetCallQueueMembers(GetCallQueueMemberRequest request);
    }

    internal class ZoomPhoneIntegration : Integration<ZoomConfiguration>, IZoomPhoneIntegration
    {
        private ZoomAuthenticationResponse? _authenticationToken = null;

        internal ZoomPhoneIntegration(IServiceProvider ioc)
            : base(ioc)
        {
        }

        public override async Task Initialize(ZoomConfiguration configuration)
        {
            await base.Initialize(configuration);

            _authenticationToken = null;
        }

        private async Task<string> Authenticate()
        {
            if ((_authenticationToken != null) && !String.IsNullOrWhiteSpace(_authenticationToken.AccessToken))
                return _authenticationToken.AccessToken;

            var ioc = Configuration.IoC;

            var clientFactory = ioc.GetRequiredService<IHttpClientFactory>();
            var stringUtils = ioc.GetRequiredService<IStringUtils>();

            using var client = clientFactory.Create();
            client.SetAuthorization(stringUtils.ToBase64($"{Configuration.ClientId}:{Configuration.ClientSecret}"));

            var route = $"https://zoom.us/oauth/token?grant_type=account_credentials&account_id={Configuration.AccountId}";
            _authenticationToken = await client.Post<ZoomAuthenticationResponse>(route);

            if ((_authenticationToken == null) || String.IsNullOrWhiteSpace(_authenticationToken.AccessToken))
                throw new InvalidOperationException();

            return _authenticationToken.AccessToken;
        }

        public async Task<GetCallQueueResponse> GetCallQueues(GetUsersRequest request)
        {
            var accessToken = await Authenticate();

            var ioc = Configuration.IoC;

            var clientFactory = ioc.GetRequiredService<IHttpClientFactory>();

            using var client = clientFactory.Create();
            client.SetAuthorization(accessToken, AuthorizationType.Bearer);

            var route = $"https://api.zoom.us/v2/phone/call_queues";
            var zoomData = await client.Get<GetCallQueueResponse>(route);

            return JsonSerializer.Deserialize<GetCallQueueResponse>(JsonSerializer.Serialize(zoomData))
                ?? throw new InvalidOperationException($"Cannot deserialize {nameof(GetUsersResponse)}");
        }

        public async Task<GetCallQueueMemberResponse> GetCallQueueMembers(GetCallQueueMemberRequest request)
        {
            var accessToken = await Authenticate();

            var ioc = Configuration.IoC;

            var clientFactory = ioc.GetRequiredService<IHttpClientFactory>();

            using var client = clientFactory.Create();
            client.SetAuthorization(accessToken, AuthorizationType.Bearer);

            var route = $"https://api.zoom.us/v2/phone/call_queues/{request.CallQeuueId}/members";
            var zoomData = await client.Get<ZoomGetCallQueueMemberResponse>(route);

            var response = JsonSerializer.Deserialize<ZoomGetCallQueueMemberResponse>(JsonSerializer.Serialize(zoomData))
                ?? throw new InvalidOperationException($"Cannot deserialize {nameof(ZoomGetCallQueueMemberResponse)}");

            return response.CallQueueMembers?.Select(_ => new GetCallQueueMemberResponse()
            {
                Id = _.Id,
                Name = _.Name,
                ExtensionId = _.ExtensionId,
                Level = _.Level,
                ReceiveCall = _.ReceiveCall
            }).FirstOrDefault() ?? new GetCallQueueMemberResponse();
        }

        //public async Task<GetCallQueueMemberResponse> SetCallQueueMembers(GetCallQueueMemberRequest request)
        //{
        //    var accessToken = await Authenticate();

        //    var ioc = Configuration.IoC;

        //    var clientFactory = ioc.GetRequiredService<IHttpClientFactory>();

        //    using var client = clientFactory.Create();
        //    client.SetAuthorization(accessToken, AuthorizationType.Bearer);

        //    var route = $"https://api.zoom.us/v2/phone/call_queues/{request.CallQeuueId}/members";
        //    //var zoomData = await client.Get<ZoomGetCallQueueMemberResponse>(route);

        //    var queueMember = new ZoomSetCallQueueMember
        //    {
        //        Members = new Members
        //        {
        //            Users = new List<PhoneUser>
        //            {
        //                new PhoneUser { Id = "SJjwzBLURpanjczk0b6bTg", Email = "ryan.wylie@s2cognition.com" }
        //            }
        //        }
        //    };

        //    var x = JsonSerializer.Serialize(queueMember);



        //    var y = await client.Post < (route);

        //    var response = JsonSerializer.Deserialize<ZoomGetCallQueueMemberResponse>(JsonSerializer.Serialize(zoomData))
        //        ?? throw new InvalidOperationException($"Cannot deserialize {nameof(ZoomGetCallQueueMemberResponse)}");

        //    return response.CallQueueMembers?.Select(_ => new GetCallQueueMemberResponse()
        //    {
        //        Id = _.Id,
        //        Name = _.Name,
        //        ExtensionId = _.ExtensionId,
        //        Level = _.Level,
        //        ReceiveCall = _.ReceiveCall
        //    }).FirstOrDefault() ?? new GetCallQueueMemberResponse();
        //}
    }
}
