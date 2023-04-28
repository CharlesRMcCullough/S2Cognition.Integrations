namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Models;

internal interface IAwsCognitoConfigFactory
{
    IAwsCognitoConfig Create();
}
internal class AwsCognitoConfigFactory : IAwsCognitoConfigFactory
{
    public IAwsCognitoConfig Create()
    {
        return new AwsCognitoConfig();
    }
}
