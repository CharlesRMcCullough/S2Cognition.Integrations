using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.AmazonWebServices.Cognito.Data;
using S2Cognition.Integrations.AmazonWebServices.Cognito.Models;
using S2Cognition.Integrations.AmazonWebServices.Core.Models;
using S2Cognition.Integrations.Core;

namespace S2Cognition.Integrations.AmazonWebServices.Cognito
{
    public interface IAmazonWebServicesCognitoIntegration : IIntegration<AmazonWebServicesCognitoConfiguration>
    {
        Task<ListCognitoUsersResponse> GetUserList(ListCognitoUsersRequest request);
        Task<CreateCognitoUserResponse> CreateUser(CreateCognitoUserRequest request);
        Task<SetCognitoPasswordResponse> SetPassword(SetCognitoPasswordRequest request);
        Task<ResetCognitoPasswordResponse> ResetPassword(ResetCognitoPasswordRequest request);
        Task<SignOutCognitoResponse> GlobalSignOut(SignOutCognitoRequest request);
        Task<SignOutCognitoResponse> ForgotPassword(ForgotCognitoPasswordRequest request);
    }
    internal class AmazonWebServicesCognitoIntegration : Integration<AmazonWebServicesCognitoConfiguration>, IAmazonWebServicesCognitoIntegration
    {
        private IAwsCognitoClient? _client;
        private IAwsCognitoClient Client
        {
            get
            {
                if (_client == null)
                {
                    var factory = _serviceProvider.GetRequiredService<IAwsCognitoClientFactory>();

                    var regionFactory = _serviceProvider.GetRequiredService<IAwsRegionFactory>();

                    _client = factory.Create(new AwsCognitoConfig
                    {
                        AccessToken = Configuration.AccessKey,
                        SecretToken = Configuration.SecretKey,
                        ServiceUrl = Configuration.ServiceUrl,
                        RegionEndpoint = regionFactory.Create(Configuration.AwsRegion).Result
                    });
                }

                return _client;
            }
        }

        internal AmazonWebServicesCognitoIntegration(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }


        //public async Task<GetUserResponse> GetUser(string email)
        //{
        //    //if (String.IsNullOrWhiteSpace(email))
        //    //    throw new S2CognitionException(S2CognitionExceptionTypes.AuthenticationUnknownCredentials);

        //    var username = ConvertEmailToUsername(email);
        //    ////using var client = BuildClient(out var awsUserPoolId);
        //    try
        //    {
        //        var response = await Client.GetUser(new AdminGetUserRequest
        //        {
        //            Username = username,
        //            UserPoolId = "us-east-2_6RDboNybw"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    //  var emailAttribute = response.UserAttributes.FirstOrDefault(_ => _.Name == "email");
        //    //return new AuthUser
        //    //{
        //    //    Username = response.Username,
        //    //    EmailAddress = emailAttribute?.Value
        //    //};
        //    return new GetUserResponse();
        //}

        public async Task<ListCognitoUsersResponse> GetUserList(ListCognitoUsersRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(ListCognitoUsersRequest));

            if (string.IsNullOrWhiteSpace(request.UserPoolId))
                throw new ArgumentException(nameof(ListCognitoUsersRequest.UserPoolId));

            IList<CognitoUserRecord> CongitoUsers = new List<CognitoUserRecord>();

            ListUsersResponse? response = null;

            response = await Client.ListUsers(new ListUsersRequest
            {
                UserPoolId = request.UserPoolId,
                Filter = null
            });

            if (response != null)
            {
                foreach (var usr in response.Users)
                {
                    CognitoUserRecord user = new CognitoUserRecord();

                    user.UserName = usr.Username;
                    user.CreatedDate = usr.UserCreateDate;
                    user.UserStatus = usr.UserStatus;

                    foreach (var attr in usr.Attributes)
                    {
                        switch (attr.Name)
                        {
                            case "email_verified":
                                user.EmailVerified = attr.Value;
                                break;
                            case "given_name":
                                user.FirstName = attr.Value;
                                break;
                            case "family_name":
                                user.LastName = attr.Value;
                                break;
                            case "email":
                                user.Email = attr.Value;
                                break;
                            case "status":
                                user.Email = attr.Value;
                                break;
                            default:
                                break;
                        }
                    }

                    CongitoUsers.Add(user);
                }
            }

            return new ListCognitoUsersResponse
            {
                UserRecords = CongitoUsers.OrderBy(_ => _.LastName).ToList()
            };
        }

        public async Task<CreateCognitoUserResponse> CreateUser(CreateCognitoUserRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(CreateCognitoUserRequest));

            var attributes = new List<AttributeType>
            {
                new AttributeType { Name = "email", Value = request.EmailAddress },
                new AttributeType { Name = "email_verified", Value = "true" }
            };

            if (!String.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                attributes.Add(new AttributeType { Name = "phone_number", Value = request.PhoneNumber });
                attributes.Add(new AttributeType { Name = "phone_number_verified", Value = "true" });
            }

            if (!String.IsNullOrWhiteSpace(request.LastName))
            {
                attributes.Add(new AttributeType { Name = "family_name", Value = request.LastName });
            }

            if (!String.IsNullOrWhiteSpace(request.FirstName))
            {
                if (!String.IsNullOrWhiteSpace(request.MiddleName))
                {
                    attributes.Add(new AttributeType { Name = "given_name", Value = $"{request.FirstName} {request.MiddleName}" });
                }
                else
                {
                    attributes.Add(new AttributeType { Name = "given_name", Value = request.FirstName });
                }
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(request.MiddleName))
                {
                    attributes.Add(new AttributeType { Name = "given_name", Value = request.MiddleName });
                }
            }

            var response = await Client.CreateUser(new AdminCreateUserRequest
            {
                Username = request.UserName,
                UserPoolId = request.UserPoolId,
                ForceAliasCreation = true,
                UserAttributes = attributes

            });

            return new CreateCognitoUserResponse
            {
                UserName = response.User.Username,
                EmailAddress = response.User.UserStatus
            };
        }

        public async Task<SetCognitoPasswordResponse> SetPassword(SetCognitoPasswordRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(CreateCognitoUserRequest));

            await Client.SetPassword(new AdminSetUserPasswordRequest
            {
                Username = request.UserName,
                Password = request.Password,
                Permanent = true,
                UserPoolId = request.UserPoolId
            });

            return new SetCognitoPasswordResponse();

        }

        public async Task<ResetCognitoPasswordResponse> ResetPassword(ResetCognitoPasswordRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(ResetCognitoPasswordRequest));

            if (string.IsNullOrEmpty(request.UserName))
                throw new ArgumentException(nameof(request.UserName));

            if (string.IsNullOrEmpty(request.UserPoolId))
                throw new ArgumentException(nameof(request.UserPoolId));

            await Client.ResetPassword(new AdminResetUserPasswordRequest
            {
                Username = request.UserName,
                UserPoolId = request.UserPoolId
            });

            return new ResetCognitoPasswordResponse();
        }

        public async Task<SignOutCognitoResponse> GlobalSignOut(SignOutCognitoRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(SignOutCognitoRequest));

            if (string.IsNullOrEmpty(request.UserName))
                throw new ArgumentException(nameof(request.UserName));

            if (string.IsNullOrEmpty(request.UserPoolId))
                throw new ArgumentException(nameof(request.UserPoolId));

            await Client.GlobalSignOut(new AdminUserGlobalSignOutRequest
            {
                Username = request.UserName,
                UserPoolId = request.UserPoolId
            });

            return new SignOutCognitoResponse();
        }

        public async Task<SignOutCognitoResponse> ForgotPassword(ForgotCognitoPasswordRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(ForgotCognitoPasswordRequest));

            if (string.IsNullOrEmpty(request.UserName))
                throw new ArgumentException(nameof(request.UserName));

            await Client.ForgotPassword(new ForgotPasswordRequest
            {
                Username = request.UserName,
                ClientId = request.ClientId

            });

            return new SignOutCognitoResponse();
        }

        private string ConvertEmailToUsername(string emailAddress)
        {
            var cleanedEmail = emailAddress.Trim();
            var username = cleanedEmail.Replace("@", "_");

            return username;
        }
    }
}
