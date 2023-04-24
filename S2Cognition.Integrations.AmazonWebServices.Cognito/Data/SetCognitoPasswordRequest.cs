namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data;

public class SetCognitoPasswordRequest
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? UserPoolId { get; set; }
}
