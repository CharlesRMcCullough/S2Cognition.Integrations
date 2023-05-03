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
        Task<CreateCognitoUserResponse> AdminCreateUser(CreateCognitoUserRequest request);
        Task<SetCognitoPasswordResponse> AdminSetPassword(SetCognitoPasswordRequest request);
        Task<ResetCognitoPasswordResponse> AdminResetPassword(ResetCognitoPasswordRequest request);
        Task<SignOutCognitoResponse> AdminGlobalSignOut(SignOutCognitoRequest request);
        Task<SignOutCognitoResponse> ForgotPassword(ForgotCognitoPasswordRequest request);
        Task<AdminUpdateUserAttributesResponse> AdminUpdateUserAttributes(AdminUpdateUserAttributesRequest request);
        Task<AdminInitiateAuthResponse> AdminInitiateAuth(AdminInitiateAuthRequest request);
        Task<InitiateAuthResponse> InitiateAuth(InitiateAuthRequest request);
        Task<ConfirmCognitoForgotPasswordResponse> ConfirmForgotPassword(ForgotCognitoPasswordRequest request);
        Task<RespondToAuthChallengeResponse> RespondToAuthCallenge(RespondToAuthChallengeRequest request);
        Task<AdminRespondToAuthChallengeResponse> AdminRespondToAuthCallenge(AdminRespondToAuthChallengeRequest request);

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
                Filter = request.Filter,
                AttributesToGet = request.AttributesToGet,
                Limit = request.Limit ?? 0,
                PaginationToken = request.PaginationToken
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

        public async Task<CreateCognitoUserResponse> AdminCreateUser(CreateCognitoUserRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(CreateCognitoUserRequest));

            if (string.IsNullOrEmpty(request.UserName))
                throw new ArgumentException(nameof(request.UserName));

            if (string.IsNullOrEmpty(request.UserPoolId))
                throw new ArgumentException(nameof(request.UserPoolId));

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

            var response = await Client.AdminCreateUser(new AdminCreateUserRequest
            {
                Username = request.UserName,
                UserPoolId = request.UserPoolId,
                ForceAliasCreation = true,
                UserAttributes = attributes,
                DesiredDeliveryMediums = request.DesiredDeliveryMediums?.ToList(),
                MessageAction = request.MessageAction,
                TemporaryPassword = request.TemporaryPassword,
                ValidationData = request.ValidationData?.ToList(),
                ClientMetadata = request.ClientMetaData
            });

            return new CreateCognitoUserResponse
            {
                UserName = response.User.Username,
                EmailAddress = response.User.UserStatus
            };
        }

        public async Task<SetCognitoPasswordResponse> AdminSetPassword(SetCognitoPasswordRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(SetCognitoPasswordRequest));

            if (string.IsNullOrEmpty(request.Password))
                throw new ArgumentException(nameof(request.Password));

            if (string.IsNullOrEmpty(request.UserPoolId))
                throw new ArgumentException(nameof(request.UserPoolId));

            await Client.AdminSetPassword(new AdminSetUserPasswordRequest
            {
                Username = request.UserName,
                Password = request.Password,
                Permanent = true,
                UserPoolId = request.UserPoolId
            });

            return new SetCognitoPasswordResponse();

        }

        public async Task<ResetCognitoPasswordResponse> AdminResetPassword(ResetCognitoPasswordRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(ResetCognitoPasswordRequest));

            if (string.IsNullOrEmpty(request.UserName))
                throw new ArgumentException(nameof(request.UserName));

            if (string.IsNullOrEmpty(request.UserPoolId))
                throw new ArgumentException(nameof(request.UserPoolId));

            await Client.AdminResetPassword(new AdminResetUserPasswordRequest
            {
                Username = request.UserName,
                UserPoolId = request.UserPoolId
            });

            return new ResetCognitoPasswordResponse();
        }

        public async Task<SignOutCognitoResponse> AdminGlobalSignOut(SignOutCognitoRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(SignOutCognitoRequest));

            if (string.IsNullOrEmpty(request.UserName))
                throw new ArgumentException(nameof(request.UserName));

            if (string.IsNullOrEmpty(request.UserPoolId))
                throw new ArgumentException(nameof(request.UserPoolId));

            await Client.AdminGlobalSignOut(new AdminUserGlobalSignOutRequest
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
                ClientId = request.ClientId,
                AnalyticsMetadata = request.AnalyticsMetadata,
                SecretHash = request.SecretHash,
                UserContextData = request.UserContextData,
                ClientMetadata = request.ClientMetaData

            });

            return new SignOutCognitoResponse();
        }

        public async Task<ConfirmCognitoForgotPasswordResponse> ConfirmForgotPassword(ForgotCognitoPasswordRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(ForgotCognitoPasswordRequest));

            if (string.IsNullOrEmpty(request.ClientId))
                throw new ArgumentException(nameof(request.ClientId));

            if (string.IsNullOrEmpty(request.UserName))
                throw new ArgumentException(nameof(request.UserName));

            if (string.IsNullOrEmpty(request.ConfirmationCode))
                throw new ArgumentException(nameof(request.ConfirmationCode));

            if (string.IsNullOrEmpty(request.Password))
                throw new ArgumentException(nameof(request.Password));

            await Client.ConfirmForgotPassword(new ConfirmForgotPasswordRequest
            {
                Username = request.UserName,
                ClientId = request.ClientId,
                AnalyticsMetadata = request.AnalyticsMetadata,
                ClientMetadata = request.ClientMetaData,
                SecretHash = request.SecretHash,
                UserContextData = request.UserContextData,
            });

            return new ConfirmCognitoForgotPasswordResponse();
        }

        public async Task<RespondToAuthChallengeResponse> RespondToAuthCallenge(RespondToAuthChallengeRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(RespondToAuthChallengeRequest));

            if (string.IsNullOrEmpty(request.ChallengeName))
                throw new ArgumentException(nameof(request.ChallengeName));

            if (string.IsNullOrEmpty(request.ClientId))
                throw new ArgumentException(nameof(request.ClientId));

            var response = await Client.RespondToAuthChallenge(new RespondToAuthChallengeRequest
            {
                ClientId = request.ClientId,
                AnalyticsMetadata = request.AnalyticsMetadata,
                ChallengeName = request.ChallengeName,
                ChallengeResponses = request.ChallengeResponses,
                ClientMetadata = request.ClientMetadata,
                UserContextData = request.UserContextData,
                Session = request.Session
            });

            return response ?? new RespondToAuthChallengeResponse();
        }

        public async Task<AdminRespondToAuthChallengeResponse> AdminRespondToAuthCallenge(AdminRespondToAuthChallengeRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(AdminRespondToAuthChallengeRequest));

            if (string.IsNullOrEmpty(request.ChallengeName))
                throw new ArgumentException(nameof(request.ChallengeName));

            if (string.IsNullOrEmpty(request.ClientId))
                throw new ArgumentException(nameof(request.ClientId));

            if (string.IsNullOrEmpty(request.UserPoolId))
                throw new ArgumentException(nameof(request.UserPoolId));

            AdminRespondToAuthChallengeResponse response;

            response = await Client.AdminRespondToAuthChallenge(new AdminRespondToAuthChallengeRequest
            {
                ClientId = request.ClientId,
                UserPoolId = request.UserPoolId,
                AnalyticsMetadata = request.AnalyticsMetadata,
                ChallengeName = request.ChallengeName,
                ChallengeResponses = request.ChallengeResponses,
                ClientMetadata = request.ClientMetadata,
                ContextData = request.ContextData,
                Session = request.Session
            });

            return response ?? new AdminRespondToAuthChallengeResponse();
        }

        public async Task<AdminInitiateAuthResponse> AdminInitiateAuth(AdminInitiateAuthRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(AdminInitiateAuthRequest));

            if (string.IsNullOrEmpty(request.AuthFlow))
                throw new ArgumentException(nameof(request.AuthFlow));

            if (string.IsNullOrEmpty(request.ClientId))
                throw new ArgumentException(nameof(request.ClientId));

            if (string.IsNullOrEmpty(request.UserPoolId))
                throw new ArgumentException(nameof(request.UserPoolId));

            var response = await Client.AdminInitiateAuth(new AdminInitiateAuthRequest
            {
                ClientId = request.ClientId,
                AuthFlow = request.AuthFlow,
                UserPoolId = request.UserPoolId,
                AnalyticsMetadata = request.AnalyticsMetadata,
                AuthParameters = request.AuthParameters,
                ClientMetadata = request.ClientMetadata
            });

            return response ?? new AdminInitiateAuthResponse();
        }

        public async Task<InitiateAuthResponse> InitiateAuth(InitiateAuthRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(InitiateAuthRequest));

            if (string.IsNullOrEmpty(request.AuthFlow))
                throw new ArgumentException(nameof(request.AuthFlow));

            if (string.IsNullOrEmpty(request.ClientId))
                throw new ArgumentException(nameof(request.ClientId));

            var response = await Client.InitiateAuth(new InitiateAuthRequest
            {
                ClientId = request.ClientId,
                AuthFlow = request.AuthFlow,
                ClientMetadata = request.ClientMetadata,
                AnalyticsMetadata = request.AnalyticsMetadata,
                AuthParameters = request.AuthParameters,
                UserContextData = request.UserContextData
            });

            return response ?? new InitiateAuthResponse();
        }

        //public async Task<AuthicateCognitoUserResponse> AuthenticateUser(AuthicateCognitoUserRequest request)
        //{
        //    if (request == null)
        //        throw new ArgumentException(nameof(AuthicateCognitoUserRequest));

        //    //if (string.IsNullOrEmpty(request.UserName))
        //    //    throw new ArgumentException(nameof(request.UserName));

        //    var response = await Client.AdminInitiateAuth(new AdminInitiateAuthRequest
        //    {
        //        ClientId = request.ClientId,
        //        AuthFlow = AuthFlowType.CUSTOM_AUTH,
        //        UserPoolId = request.UserPoolId,
        //        AuthParameters = new Dictionary<string, string>()
        //        {
        //            { "USERNAME", request.Username ?? string.Empty }
        //        }
        //    });

        //    var code = request.AccessCode;
        //    if (string.IsNullOrEmpty(code))
        //        code = request.ShortCode;

        //    // Only one challenge, but this could be multiple for sending email codes, captcha, etc
        //    var challengeResponse = await Client.AdminRespondToAuthChallenge(new AdminRespondToAuthChallengeRequest
        //    {
        //        ClientId = request.ClientId,
        //        ChallengeName = ChallengeNameType.CUSTOM_CHALLENGE,
        //        Session = response.Session,
        //        UserPoolId = request.UserPoolId,
        //        ChallengeResponses = new Dictionary<string, string>()
        //        {
        //            { "USERNAME", request.Username ?? string.Empty },
        //            { "ANSWER", code ?? string.Empty }
        //        }
        //    });

        //    return new AuthicateCognitoUserResponse
        //    {
        //        AccessToken = challengeResponse.AuthenticationResult.AccessToken,
        //        IdentityToken = challengeResponse.AuthenticationResult.IdToken,
        //        Username = request.Username
        //    };
        //}


        public async Task<AdminUpdateUserAttributesResponse> AdminUpdateUserAttributes(AdminUpdateUserAttributesRequest request)
        {
            if (request == null)
                throw new ArgumentException(nameof(AdminUpdateUserAttributesRequest));

            if (request.UserAttributes == null)
                throw new ArgumentException(nameof(request.UserAttributes));

            if (string.IsNullOrEmpty(request.Username))
                throw new ArgumentException(nameof(request.Username));

            if (string.IsNullOrEmpty(request.UserPoolId))
                throw new ArgumentException(nameof(request.UserPoolId));

            AdminUpdateUserAttributesResponse response;

            //if (request.OldUserName != request.NewUserName)
            //{
            //    var cognitoUser = await Client.ListUsers(new ListUsersRequest
            //    {
            //        Filter = $"email=\"{request.OldUserName}\"",
            //        UserPoolId = request.UserPoolId
            //    });

            //var response = await Client.AdminUpdateUserAttributes(new AdminUpdateUserAttributesRequest
            //    {
            //        UserAttributes = new List<AttributeType>
            //        {
            //            new AttributeType{Name="email", Value=request.NewUserName},
            //            new AttributeType{Name="email_verified", Value="true"}
            //        },
            //        UserPoolId = request.NewUserName,
            //        Username = cognitoUser.Users[0].Username
            //    });

            //    return new ChangeCognitoUserNameResponse();
            //}

            response = await Client.AdminUpdateUserAttributes(new AdminUpdateUserAttributesRequest
            {
                UserPoolId = request.UserPoolId,
                UserAttributes = request.UserAttributes,
                ClientMetadata = request.ClientMetadata,
                Username = request.Username
            });

            return response ?? new AdminUpdateUserAttributesResponse();
        }

        private string ConvertEmailToUsername(string emailAddress)
        {
            var cleanedEmail = emailAddress.Trim();
            var username = cleanedEmail.Replace("@", "_");

            return username;
        }
    }
}
