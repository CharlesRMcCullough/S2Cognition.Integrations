namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data;

public class AuthicateCognitoUserResponse
{
    public string? Username { get; set; }
    public string? AccessToken { get; set; }
    public string? IdentityToken { get; set; }
}
