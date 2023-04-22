using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using System.Net;

namespace S2Cognition.Integrations.AmazonWebServices.Ssm.Models;

internal interface IAwsSsmClient
{
    Task<GetParameterResponse> GetParameter(GetParameterRequest req);
    Task<PutParameterResponse> PutParameter(PutParameterRequest req);
}

internal class AwsSsmClient : IAwsSsmClient
{
    private readonly IAwsSsmConfig _config;

    internal AwsSsmClient(IAwsSsmConfig config)
    {
        _config = config;
    }

    public async Task<GetParameterResponse> GetParameter(GetParameterRequest req)
    {
        try
        {
            using var client = new AmazonSimpleSystemsManagementClient(_config.Native);
            return await client.GetParameterAsync(req);
        }
        catch (ParameterNotFoundException)
        {
            return new GetParameterResponse { HttpStatusCode = HttpStatusCode.NotFound };
        }
        catch (Exception)
        {
            return new GetParameterResponse { HttpStatusCode = HttpStatusCode.InternalServerError };
        }
    }

    public async Task<PutParameterResponse> PutParameter(PutParameterRequest req)
    {
        using var client = new AmazonSimpleSystemsManagementClient(_config.Native);
        return await client.PutParameterAsync(req);
    }
}
