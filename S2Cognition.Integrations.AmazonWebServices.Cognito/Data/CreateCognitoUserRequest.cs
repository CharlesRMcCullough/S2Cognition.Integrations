using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;

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
    public IList<string>? DesiredDeleveryMediums { get; set; }
    public bool ForceAliasCreation { get; set; }
    public IList<AttributeType>? UserAttributes { get; set; }
    public IList<AttributeType>? ValidationData { get; set; }
    public Dictionary<string, string>? ClientMetaData { get; set; }


}
