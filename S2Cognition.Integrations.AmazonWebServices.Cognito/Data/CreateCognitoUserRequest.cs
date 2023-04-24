using Amazon.CognitoIdentityProvider;

namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data;

public class CreateCognitoUserRequest
{
    public string? UserPoolId { get; set; }
    public string? UserName { get; set; }
    public IList<string>? DesiredDeliveryMediums { get; set; }
    public MessageActionType? MessageAction { get; set; }
    public string? TemporaryPassword { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? EmailAddress { get; set; }
    public string? PhoneNumber { get; set; }


}
