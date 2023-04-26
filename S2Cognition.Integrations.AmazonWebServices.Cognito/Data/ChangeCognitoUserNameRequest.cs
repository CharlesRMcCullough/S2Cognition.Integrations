namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data;

public class ChangeCognitoUserNameRequest
{
    public string? NewUserName { get; set; }
    public string? OldUserName { get; set; }
    public string? UserPoolId { get; set; }
}
