namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data;

public class AuthicateCognitoUserRequest
{
    public string? ClientId { get; set; }
    public string? Username { get; set; }
    public string? AccessCode { get; set; }
    //public string? IdentityToken { get; set; }
    public string? UserPoolId { get; set; }
    public string? ShortCode { get; set; }
}
