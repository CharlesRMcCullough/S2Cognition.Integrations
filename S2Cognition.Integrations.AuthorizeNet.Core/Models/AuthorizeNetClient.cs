using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;

using AdnEnv = AuthorizeNet.Environment;

namespace S2Cognition.Integrations.AuthorizeNet.Core.Models;

public interface IAuthorizeNetClient
{
    Task Initialize(AuthorizeNetConfiguration configuration);
    Task<createTransactionResponse> CreateTransaction(createTransactionRequest request);
}

public class AuthorizeNetClient : IAuthorizeNetClient
{
    private AuthorizeNetConfiguration? _configuration = null;

    internal AuthorizeNetClient()
    { 
    }

    public async Task Initialize(AuthorizeNetConfiguration configuration)
    {
        _configuration = configuration;

        ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType
        {
            name = configuration.LoginId,
            ItemElementName = ItemChoiceType.transactionKey,
            Item = configuration.TransactionKey,
        };

        ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AdnEnv.SANDBOX;

        await Task.CompletedTask;
    }

    public async Task<createTransactionResponse> CreateTransaction(createTransactionRequest request)
    {
        if (_configuration == null)
            throw new InvalidOperationException(nameof(_configuration));

        var controller = new createTransactionController(request);
        controller.Execute();
        return await Task.FromResult(controller.GetApiResponse());
    }
}
