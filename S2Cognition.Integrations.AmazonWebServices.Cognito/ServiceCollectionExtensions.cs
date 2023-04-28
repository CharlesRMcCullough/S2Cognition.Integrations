using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.AmazonWebServices.Cognito.Models;
using S2Cognition.Integrations.AmazonWebServices.Core.Models;

namespace S2Cognition.Integrations.AmazonWebServices.Cognito;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAmazonWebServicesCognitoIntegration(this IServiceCollection sc)
    {
        return sc.AddSingleton<IAmazonWebServicesCognitoIntegration>(_ => new AmazonWebServicesCognitoIntegration(_))
           .AddSingleton<IAwsCognitoConfigFactory>(_ => new AwsCognitoConfigFactory())
           .AddSingleton<IAwsCognitoClientFactory>(_ => new AwsCognitoClientFactory())
           .AddSingleton<IAwsRegionFactory>(_ => new AwsRegionFactory());
    }

}
