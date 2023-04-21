namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Models;

internal interface IAwsCognitoClientFactory
{
    IAwsCognitoClient Create(IAwsCognitoConfig config);
}
internal class AwsCognitoClientFactory : IAwsCognitoClientFactory
{
    public IAwsCognitoClient Create(IAwsCognitoConfig config)
    {
        return new AwsCognitoClient(config);
    }
}
