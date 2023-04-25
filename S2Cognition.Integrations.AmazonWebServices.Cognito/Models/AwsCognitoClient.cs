using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using S2Cognition.Integrations.AmazonWebServices.Cognito.Data;

namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Models
{
    internal interface IAwsCognitoClient
    {
        AmazonCognitoIdentityProviderClient Native { get; }
        Task<GetCognitoUserResponse> GetUser(AdminGetUserRequest request);
        Task<ListUsersResponse> ListUsers(ListUsersRequest request);
        Task<AdminCreateUserResponse> CreateUser(AdminCreateUserRequest request);
        Task<AdminSetUserPasswordResponse> SetPassword(AdminSetUserPasswordRequest request);
        Task<AdminResetUserPasswordResponse> ResetPassword(AdminResetUserPasswordRequest request);
        Task<AdminUserGlobalSignOutResponse> GlobalSignOut(AdminUserGlobalSignOutRequest request);
        Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest request);

    }
    internal class AwsCognitoClient : IAwsCognitoClient
    {
        private readonly AmazonCognitoIdentityProviderClient _client;

        public AmazonCognitoIdentityProviderClient Native => _client;

        internal AwsCognitoClient(IAwsCognitoConfig config)
        {
            _client = new AmazonCognitoIdentityProviderClient(config.AccessToken, config.SecretToken, config.Native);
        }

        public async Task<GetCognitoUserResponse> GetUser(AdminGetUserRequest request)
        {

            AdminGetUserResponse response;
            try
            {
                response = await Native.AdminGetUserAsync(request);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return new GetCognitoUserResponse();
        }
        public async Task<ListUsersResponse> ListUsers(ListUsersRequest request)
        {
            ListUsersResponse response;

            response = await Native.ListUsersAsync(request);

            return response ?? new ListUsersResponse();
        }

        public async Task<AdminCreateUserResponse> CreateUser(AdminCreateUserRequest request)
        {
            AdminCreateUserResponse response;

            response = await Native.AdminCreateUserAsync(request);

            return response ?? new AdminCreateUserResponse();
        }

        public async Task<AdminSetUserPasswordResponse> SetPassword(AdminSetUserPasswordRequest request)
        {
            AdminSetUserPasswordResponse? response = null;

            try
            {
                response = await Native.AdminSetUserPasswordAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response ?? new AdminSetUserPasswordResponse();
        }

        public async Task<AdminResetUserPasswordResponse> ResetPassword(AdminResetUserPasswordRequest request)
        {
            AdminResetUserPasswordResponse? response = null;

            try
            {
                response = await Native.AdminResetUserPasswordAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response ?? new AdminResetUserPasswordResponse();
        }

        public async Task<AdminUserGlobalSignOutResponse> GlobalSignOut(AdminUserGlobalSignOutRequest request)
        {
            AdminUserGlobalSignOutResponse response;

            response = await Native.AdminUserGlobalSignOutAsync(request);

            return response ?? new AdminUserGlobalSignOutResponse();
        }

        public async Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest request)
        {
            ForgotPasswordResponse response;

            response = await Native.ForgotPasswordAsync(request);

            return response ?? new ForgotPasswordResponse();
        }

        public async Task<AdminInitiateAuthResponse> InitiateAuth(AdminInitiateAuthRequest request)
        {
            AdminInitiateAuthResponse response;

            response = await Native.AdminInitiateAuthAsync(request);

            return response ?? new AdminInitiateAuthResponse();
        }

        public async Task<AdminRespondToAuthChallengeResponse> RespondToAuthChallenge(AdminRespondToAuthChallengeRequest request)
        {
            AdminRespondToAuthChallengeResponse response;

            response = await Native.AdminRespondToAuthChallengeAsync(request);

            return response ?? new AdminRespondToAuthChallengeResponse();
        }
    }
}
