
using S2Cognition.Integrations.AmazonWebServices.Core.Data;

namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data;

public class AmazonWebServicesCognitoConfiguration : AmazonWebServicesConfiguration
{
    public string ServiceUrl { get; set; } = string.Empty;

    public AmazonWebServicesCognitoConfiguration(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }
}
