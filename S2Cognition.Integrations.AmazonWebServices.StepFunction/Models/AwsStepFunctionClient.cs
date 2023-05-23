using Amazon.StepFunctions.Model;
using Amazon.StepFunctions;

namespace S2Cognition.Integrations.AmazonWebServices.StepFunction.Models;

public interface IAwsStepFunctionClient
{
    Task<StartExecutionResponse> Execute(StartExecutionRequest req);
}

internal class AwsStepFunctionClient : IAwsStepFunctionClient
{
    public async Task<StartExecutionResponse> Execute(StartExecutionRequest req)
    {
        var client = new AmazonStepFunctionsClient();

        return await client.StartExecutionAsync(req);
    }
}