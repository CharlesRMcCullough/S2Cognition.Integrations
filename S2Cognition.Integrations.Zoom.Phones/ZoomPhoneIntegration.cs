using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.Core;
using S2Cognition.Integrations.Zoom.Core.Data;
using S2Cognition.Integrations.Zoom.Core.Models;
using S2Cognition.Integrations.Zoom.Phones.Data;

namespace S2Cognition.Integrations.Zoom.Phones
{
    public interface IZoomPhoneIntegration : IIntegration<ZoomPhoneConfiguration>
    {
        Task<GetCallQueuesResponse> GetCallQueues(GetCallQueuesRequest req);
        Task<GetCallQueueMemberResponse> GetCallQueueMembers(GetCallQueueMemberRequest req);
        Task<GetUsersResponse> GetUsers(GetUsersRequest req);
        Task<SetCallQueueMemberResponse> SetCallQueueMembers(SetCallQueueMemberRequest req);
        Task<RemoveCallQueueMemberResponse> RemoveCallQueueMembers(RemoveCallQueueMemberRequest req);
        Task<ClearCallQueueMemberResponse> ClearCallQueueMembers(ClearCallQueueMemberRequest req);
    }

    public class ZoomPhoneIntegration : Integration<ZoomPhoneConfiguration>, IZoomPhoneIntegration
    {
        private ZoomAuthenticationResponse? _authenticationToken = null;

        internal ZoomPhoneIntegration(IServiceProvider ioc)
            : base(ioc)
        {
        }

        public override async Task Initialize(ZoomPhoneConfiguration configuration)
        {
            await base.Initialize(configuration);

            _authenticationToken = null;

            var client = _serviceProvider.GetRequiredService<IZoomPhoneNativeClient>();
            await client.Initialize(new ZoomConfiguration(_serviceProvider)
            {
                AccountId = configuration.AccountId,
                ClientId = configuration.ClientId,
                ClientSecret = configuration.ClientSecret
            });
        }

        protected internal virtual async Task<string> Authenticate()
        {
            if ((_authenticationToken != null)
                && (_authenticationToken.AccessToken != null)
                && !String.IsNullOrWhiteSpace(_authenticationToken.AccessToken))
            {
                return _authenticationToken.AccessToken;
            }

            var client = _serviceProvider.GetRequiredService<IZoomPhoneNativeClient>();
            _authenticationToken = await client.Authenticate();

            if ((_authenticationToken == null)
                || (_authenticationToken.AccessToken == null)
                || String.IsNullOrWhiteSpace(_authenticationToken.AccessToken))
            {
                throw new InvalidOperationException();
            }

            return _authenticationToken.AccessToken;
        }

        public async Task<GetCallQueuesResponse> GetCallQueues(GetCallQueuesRequest req)
        {
            var accessToken = await Authenticate();
            var client = _serviceProvider.GetRequiredService<IZoomPhoneNativeClient>();

            var responseQueues = await client.GetZoomCallQueues(accessToken, new GetCallQueuesRequest());

            var queuesToReturn = new List<CallQueueRecord>();

            if (responseQueues != null && responseQueues.CallQueues != null)
            {
                foreach (var queue in responseQueues.CallQueues)
                {
                    queuesToReturn.Add(new CallQueueRecord
                    {
                        id = queue.id,
                        Name = queue.Name,
                        PhoneNumber = queue.PhoneNumber != null ? queue.PhoneNumber.Select(_ => new PhoneNumberRecord
                        {
                            Id = _.Id,
                            Number = _.Number,
                            Source = _.Source
                        }).ToList() : null,
                        Extension_Id = queue.Extension_Id,
                        ExtensionNumber = queue.ExtensionNumber,
                        Status = queue.Status,
                        Site = queue.Site != null ? new SiteRecord
                        {
                            Id = queue.Site.Id,
                            Name = queue.Site.Name,
                        } : null
                    });
                }
            }

            return new GetCallQueuesResponse
            {
                CallQueues = queuesToReturn
            };
        }

        public async Task<GetCallQueueMemberResponse> GetCallQueueMembers(GetCallQueueMemberRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.CallQueueName))
                throw new ArgumentException(nameof(GetCallQueueMemberRequest.CallQueueName));

            string? _queueId = null;
            var accessToken = await Authenticate();
            var client = _serviceProvider.GetRequiredService<IZoomPhoneNativeClient>();

            var queues = await client.GetZoomCallQueues(accessToken, new GetCallQueuesRequest());

            if (req.CallQueueName != null && queues.CallQueues != null)
            {
                _queueId = queues.CallQueues.First(_ => _.Name?.ToLower().Trim() == req.CallQueueName.ToLower().Trim()).id;
            }

            var responseMemberQueues = await client.GetZoomCallQueueMembers(accessToken, new ZoomGetCallQueueMemberRequest
            {
                CallQueueId = _queueId
            });

            return responseMemberQueues.CallQueueMembers?.Select(_ => new GetCallQueueMemberResponse()
            {
                Id = _.Id,
                Name = _.Name,
                ExtensionId = _.ExtensionId,
                Level = _.Level,
                ReceiveCall = _.ReceiveCall,

            }).FirstOrDefault() ?? new GetCallQueueMemberResponse();
        }

        public async Task<GetUsersResponse> GetUsers(GetUsersRequest req)
        {
            var accessToken = await Authenticate();
            var client = _serviceProvider.GetRequiredService<IZoomPhoneNativeClient>();

            var userResponse = await client.GetZoomUsers(accessToken, req);

            var users = new List<UserRecord>();

            if (userResponse.Users != null)
            {
                users.AddRange(userResponse.Users
                    .Select(_ => new UserRecord
                    {
                        Id = _.Id ?? "",
                        Department = _.Department,
                        Email = _.Email,
                        FirstName = _.FirstName,
                        LastName = _.LastName
                    }));
            }

            var usersToReturn = new GetUsersResponse
            {
                Users = users,
                NextPageToken = userResponse.NextPageToken,
                PageSize = userResponse.PageSize,
                TotalRecords = userResponse.TotalRecords
            };

            return usersToReturn;
        }

        public async Task<SetCallQueueMemberResponse> SetCallQueueMembers(SetCallQueueMemberRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.QueueName))
                throw new ArgumentException(nameof(SetCallQueueMemberRequest.QueueName));

            if (string.IsNullOrWhiteSpace(req.UserEmail))
                throw new ArgumentException(nameof(SetCallQueueMemberRequest.UserEmail));

            string? _userId = null;
            string? _queueId = null;

            var accessToken = await Authenticate();
            var client = _serviceProvider.GetRequiredService<IZoomPhoneNativeClient>();

            var users = await client.GetZoomUsers(accessToken, new GetUsersRequest());

            if (req.UserEmail != null && users.Users != null)
            {
                _userId = users.Users.First(_ => _.Email?.ToLower().Trim() == req.UserEmail.ToLower().Trim()).Id;
            }

            var queues = await client.GetZoomCallQueues(accessToken, new GetCallQueuesRequest());

            if (req.QueueName != null && queues.CallQueues != null)
            {
                _queueId = queues.CallQueues.First(_ => _.Name?.ToLower().Trim() == req.QueueName.ToLower().Trim()).id;
            }

            await client.SetZoomCallQueueMembers(accessToken, new SetZoomCallQueueMemberRequest
            {
                CallQeuueId = _queueId,
                UserId = _userId,
                UserEmail = req.UserEmail,
            });

            return new SetCallQueueMemberResponse();
        }

        public async Task<RemoveCallQueueMemberResponse> RemoveCallQueueMembers(RemoveCallQueueMemberRequest req)
        {

            if (string.IsNullOrWhiteSpace(req.QueueName))
                throw new ArgumentException(nameof(RemoveCallQueueMemberRequest.QueueName));

            if (string.IsNullOrWhiteSpace(req.UserEmail))
                throw new ArgumentException(nameof(RemoveCallQueueMemberRequest.UserEmail));

            string? _userId = null;
            string? _queueId = null;

            var accessToken = await Authenticate();
            var client = _serviceProvider.GetRequiredService<IZoomPhoneNativeClient>();

            var users = await client.GetZoomUsers(accessToken, new GetUsersRequest());

            if (req.UserEmail != null && users.Users != null)
            {
                _userId = users.Users.First(_ => _.Email?.ToLower().Trim() == req.UserEmail.ToLower().Trim()).Id;
            }

            var queues = await client.GetZoomCallQueues(accessToken, new GetCallQueuesRequest());

            if (req.QueueName != null && queues.CallQueues != null)
            {
                _queueId = queues.CallQueues.First(_ => _.Name?.ToLower().Trim() == req.QueueName.ToLower().Trim()).id;
            }

            if (_queueId != null)
            {
                await client.RemoveZoomCallQueueMembers(accessToken, new RemoveZoomCallQueueMemberRequest
                {
                    CallQeuueId = _queueId,
                    UserId = _userId
                });

            }

            return new RemoveCallQueueMemberResponse();
        }

        public async Task<ClearCallQueueMemberResponse> ClearCallQueueMembers(ClearCallQueueMemberRequest req)
        {

            if (string.IsNullOrWhiteSpace(req.QueueName))
                throw new ArgumentException(nameof(ClearCallQueueMemberRequest.QueueName));

            string? _queueId = null;

            var accessToken = await Authenticate();
            var client = _serviceProvider.GetRequiredService<IZoomPhoneNativeClient>();

            var queues = await client.GetZoomCallQueues(accessToken, new GetCallQueuesRequest());

            if (req.QueueName != null && queues.CallQueues != null)
            {
                _queueId = queues.CallQueues.First(_ => _.Name?.ToLower().Trim() == req.QueueName.ToLower().Trim()).id;
            }

            if (_queueId != null)
            {
                await client.ClearZoomCallQueueMembers(accessToken, new ClearZoomCallQueueMemberRequest
                {
                    CallQeuueId = _queueId
                });

            }

            return new ClearCallQueueMemberResponse();
        }
    }
}
