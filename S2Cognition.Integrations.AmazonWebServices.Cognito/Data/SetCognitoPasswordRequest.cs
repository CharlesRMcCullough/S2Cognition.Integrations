namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data;

public class SetCognitoPasswordRequest
{
    public string? UserName { get; set; } = null;
    public string? Password { get; set; } = null;
    public string? UserPoolId { get; set; } = null;
}
