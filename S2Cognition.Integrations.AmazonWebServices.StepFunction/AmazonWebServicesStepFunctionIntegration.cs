using Amazon.StepFunctions.Model;
using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.AmazonWebServices.StepFunction.Data;
using S2Cognition.Integrations.AmazonWebServices.StepFunction.Models;
using S2Cognition.Integrations.Core;
using System.Text.Json;

namespace S2Cognition.Integrations.AmazonWebServices.StepFunction;

public interface IAmazonWebServicesStepFunctionIntegration : IIntegration<AmazonWebServicesStepFunctionConfiguration>
{
    Task<ExecuteResponse> Execute(ExecuteRequest req);
}

public class AmazonWebServicesStepFunctionIntegration : Integration<AmazonWebServicesStepFunctionConfiguration>, IAmazonWebServicesStepFunctionIntegration
{
    private IAwsStepFunctionClient? _client;
    private IAwsStepFunctionClient Client
    {
        get
        {
            if (_client == null)
            {
                _client = _serviceProvider.GetRequiredService<IAwsStepFunctionClient>();
            }

            return _client;
        }
    }

    internal AmazonWebServicesStepFunctionIntegration(IServiceProvider serviceProvider)
    : base(serviceProvider)
    {
    }

    public async Task<ExecuteResponse> Execute(ExecuteRequest req)
    {
        await Client.Execute(new StartExecutionRequest 
        {
            Input = JsonSerializer.Serialize(req.Payload),
            StateMachineArn = req.Arn
        });

        return new ExecuteResponse { };
    }
}
