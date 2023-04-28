using Amazon.CognitoIdentityProvider;

namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data;

public class InitCognitoAuthRequest
{
    public string? ClientId { get; set; }
    public AuthFlowType? AuthFlowType { get; set; }
    public string? UserPoolId { get; set; }
    public Dictionary<string, string>? AuthParameters { get; set; }

}
