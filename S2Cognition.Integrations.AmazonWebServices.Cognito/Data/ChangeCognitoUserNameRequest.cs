using Amazon.CognitoIdentityProvider.Model;

namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data;

public class ChangeCognitoUserNameRequest
{
    public string? UserName { get; set; } = null;
    public string? UserPoolId { get; set; } = null;
    public Dictionary<string, string>? ClientMetaData { get; set; } = null;
    public IList<AttributeType>? UserAttributes { get; set; } = null;
}
