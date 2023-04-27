using Amazon.CognitoIdentityProvider.Model;

namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data;

public class ForgotCognitoPasswordRequest
{
    public string? ClientId { get; set; } = null;
    public string? UserName { get; set; } = null;
    public string? Password { get; set; } = null;
    public AnalyticsMetadataType? AnalyticsMetadata { get; set; } = null;
    public string? SecretHash { get; set; } = null;
    public UserContextDataType? UserContextData { get; set; } = null;
    public Dictionary<string, string>? ClientMetaData { get; set; }
    public string? ConfirmationCode { get; set; } = null;

}
