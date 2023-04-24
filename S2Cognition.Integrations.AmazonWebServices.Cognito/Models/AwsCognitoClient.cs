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
            //{
            //    AttributesToGet = request.AttributesToGet,
            //    UserPoolId = request.UserPoolId
            //});

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
            AdminSetUserPasswordResponse response;

            response = await Native.AdminSetUserPasswordAsync(request);

            return response ?? new AdminSetUserPasswordResponse();
        }
    }
}
