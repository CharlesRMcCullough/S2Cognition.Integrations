using S2Cognition.Integrations.AmazonWebServices.Core.Data;

namespace S2Cognition.Integrations.AmazonWebServices.StepFunction.Data;

public class AmazonWebServicesStepFunctionConfiguration : AmazonWebServicesConfiguration
{
    public string ServiceUrl { get; set; } = string.Empty;

    public AmazonWebServicesStepFunctionConfiguration(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }
}
