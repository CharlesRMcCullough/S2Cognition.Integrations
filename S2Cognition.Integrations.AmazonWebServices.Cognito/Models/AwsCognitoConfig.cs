using Amazon.CognitoIdentityProvider;
using S2Cognition.Integrations.AmazonWebServices.Core.Models;

namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Models;

internal interface IAwsCognitoConfig
{
    string? AccessToken { get; set; }
    string? SecretToken { get; set; }
    string? ServiceUrl { get; set; }
    IAwsRegionEndpoint? RegionEndpoint { get; set; }
    AmazonCognitoIdentityProviderConfig Native { get; }
}

internal class AwsCognitoConfig : IAwsCognitoConfig
{
    public string? AccessToken { get; set; }
    public string? SecretToken { get; set; }

    private string? _serviceUrl;
    public string? ServiceUrl
    {
        get => _serviceUrl;

        set
        {
            _serviceUrl = value;
            _config.ServiceURL = value;
        }
    }

    private IAwsRegionEndpoint? _regionEndpoint;
    public IAwsRegionEndpoint? RegionEndpoint
    {
        get => _regionEndpoint;

        set
        {
            _regionEndpoint = value;
            _config.RegionEndpoint = value?.Native;
        }
    }

    private readonly AmazonCognitoIdentityProviderConfig _config;
    public AmazonCognitoIdentityProviderConfig Native => _config;

    internal AwsCognitoConfig()
    {
        _config = new AmazonCognitoIdentityProviderConfig { ServiceURL = ServiceUrl, RegionEndpoint = RegionEndpoint?.Native };
    }
}
