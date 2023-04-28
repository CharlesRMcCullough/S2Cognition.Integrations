using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using S2Cognition.Integrations.AmazonWebServices.Cognito.Data;

namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Models
{
    internal interface IAwsCognitoClient
    {
        AmazonCognitoIdentityProviderClient Native { get; }
        Task<GetCognitoUserResponse> AdminGetUser(AdminGetUserRequest request);
        Task<ListUsersResponse> ListUsers(ListUsersRequest request);
        Task<AdminCreateUserResponse> AdminCreateUser(AdminCreateUserRequest request);
        Task<AdminSetUserPasswordResponse> AdminSetPassword(AdminSetUserPasswordRequest request);
        Task<AdminResetUserPasswordResponse> AdminResetPassword(AdminResetUserPasswordRequest request);
        Task<AdminUserGlobalSignOutResponse> AdminGlobalSignOut(AdminUserGlobalSignOutRequest request);
        Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest request);
        Task<ConfirmForgotPasswordResponse> ConfirmForgotPassword(ConfirmForgotPasswordRequest request);
        Task<AdminInitiateAuthResponse> AdminInitiateAuth(AdminInitiateAuthRequest request);
        Task<AdminRespondToAuthChallengeResponse> AdminRespondToAuthChallenge(AdminRespondToAuthChallengeRequest request);
        Task<RespondToAuthChallengeResponse> RespondToAuthChallenge(RespondToAuthChallengeRequest request);
        Task<AdminUpdateUserAttributesResponse> AdminUpdateUserAttributes(AdminUpdateUserAttributesRequest request);
        Task<InitiateAuthResponse> InitiateAuth(InitiateAuthRequest request);

    }
    internal class AwsCognitoClient : IAwsCognitoClient
    {
        private readonly AmazonCognitoIdentityProviderClient _client;

        public AmazonCognitoIdentityProviderClient Native => _client;

        internal AwsCognitoClient(IAwsCognitoConfig config)
        {
            _client = new AmazonCognitoIdentityProviderClient(config.AccessToken, config.SecretToken, config.Native);
        }

        public async Task<GetCognitoUserResponse> AdminGetUser(AdminGetUserRequest request)
        {
            var response = await Native.AdminGetUserAsync(request);
            return new GetCognitoUserResponse
            {
                Email = response.Username,
                UserName = response.Username
            };
        }

        public async Task<ListUsersResponse> ListUsers(ListUsersRequest request)
        {
            var response = await Native.ListUsersAsync(request);
            return response ?? new ListUsersResponse();
        }

        public async Task<AdminCreateUserResponse> AdminCreateUser(AdminCreateUserRequest request)
        {
            var response = await Native.AdminCreateUserAsync(request);
            return response ?? new AdminCreateUserResponse();
        }

        public async Task<AdminSetUserPasswordResponse> AdminSetPassword(AdminSetUserPasswordRequest request)
        {
            var response = await Native.AdminSetUserPasswordAsync(request);
            return response ?? new AdminSetUserPasswordResponse();
        }

        public async Task<AdminResetUserPasswordResponse> AdminResetPassword(AdminResetUserPasswordRequest request)
        {
            var response = await Native.AdminResetUserPasswordAsync(request);
            return response ?? new AdminResetUserPasswordResponse();
        }

        public async Task<AdminUserGlobalSignOutResponse> AdminGlobalSignOut(AdminUserGlobalSignOutRequest request)
        {
            var response = await Native.AdminUserGlobalSignOutAsync(request);
            return response ?? new AdminUserGlobalSignOutResponse();
        }

        public async Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest request)
        {
            var response = await Native.ForgotPasswordAsync(request);
            return response ?? new ForgotPasswordResponse();
        }

        public async Task<ConfirmForgotPasswordResponse> ConfirmForgotPassword(ConfirmForgotPasswordRequest request)
        {
            var response = await Native.ConfirmForgotPasswordAsync(request);
            return response ?? new ConfirmForgotPasswordResponse();
        }

        public async Task<InitiateAuthResponse> InitiateAuth(InitiateAuthRequest request)
        {
            var response = await Native.InitiateAuthAsync(request);
            return response ?? new InitiateAuthResponse();
        }

        public async Task<AdminInitiateAuthResponse> AdminInitiateAuth(AdminInitiateAuthRequest request)
        {
            var response = await Native.AdminInitiateAuthAsync(request);
            return response ?? new AdminInitiateAuthResponse();
        }

        public async Task<AdminRespondToAuthChallengeResponse> AdminRespondToAuthChallenge(AdminRespondToAuthChallengeRequest request)
        {
            var response = await Native.AdminRespondToAuthChallengeAsync(request);
            return response ?? new AdminRespondToAuthChallengeResponse();
        }

        public async Task<RespondToAuthChallengeResponse> RespondToAuthChallenge(RespondToAuthChallengeRequest request)
        {
            var response = await Native.RespondToAuthChallengeAsync(request);
            return response ?? new RespondToAuthChallengeResponse();
        }

        public async Task<AdminUpdateUserAttributesResponse> AdminUpdateUserAttributes(AdminUpdateUserAttributesRequest request)
        {
            var response = await Native.AdminUpdateUserAttributesAsync(request);
            return response ?? new AdminUpdateUserAttributesResponse();
        }
    }
}



