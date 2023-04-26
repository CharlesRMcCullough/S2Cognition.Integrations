namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data;

public class ConfirmCognitoForgotPasswordRequest
{
    public string? Password { get; set; }
    public string? ClientId { get; set; }
    public string? UserName { get; set; }
}
