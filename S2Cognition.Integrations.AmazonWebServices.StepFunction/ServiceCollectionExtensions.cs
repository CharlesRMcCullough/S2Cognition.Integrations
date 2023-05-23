using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.AmazonWebServices.StepFunction.Models;

namespace S2Cognition.Integrations.AmazonWebServices.StepFunction;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAmazonWebServicesStepFunctionIntegration(this IServiceCollection sc)
    {
        return sc.AddSingleton<IAmazonWebServicesStepFunctionIntegration>(_ => new AmazonWebServicesStepFunctionIntegration(_))
            .AddSingleton<IAwsStepFunctionClient, AwsStepFunctionClient>();
    }
}
