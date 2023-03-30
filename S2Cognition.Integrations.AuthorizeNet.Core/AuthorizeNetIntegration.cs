using AuthorizeNet.Api.Contracts.V1;
using S2Cognition.Integrations.AuthorizeNet.Core.Models;

namespace S2Cognition.Integrations.AuthorizeNet.Core;

public class AuthorizeNetIntegration : Integration<AuthorizeNetConfiguration>, IAuthorizeNetIntegration
{
    public const string HostedFormTokenIdentifier = "COMMON.ACCEPT.INAPP.PAYMENT";

    private IAuthorizeNetClient Client
    {
        get
        {
            return _serviceProvider.GetRequiredService<IAuthorizeNetClient>();
        }
    }

    internal AuthorizeNetIntegration(IServiceProvider serviceProvider)
        : base(serviceProvider)
	{
    }

    public override async Task Initialize(AuthorizeNetConfiguration configuration)
    {
        await base.Initialize(configuration);
        await Client.Initialize(configuration);
    }

    public async Task<OneTimePaymentResponse> OneTimePayment(OneTimePaymentRequest req)
    {
        var total = 0.0;
        var items = new List<lineItemType>();
        foreach (var item in req.Items)
        {
            var quantity = item.Quantity ?? 1;
            var unitPrice = item.UnitPrice ?? 0.0;
            total += unitPrice * quantity;
            items.Add(new lineItemType 
            {
                itemId = item.ItemId,
                name = item.Name,
                description = item.Description,
                quantity = quantity,
                unitPrice = (decimal)unitPrice
            });
        }

        var request = new createTransactionRequest
        {
            transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),
                amount = (decimal)total,
                amountSpecified = true,
                payment = new paymentType
                {
                    Item = new opaqueDataType
                    {
                        dataDescriptor = HostedFormTokenIdentifier,
                        dataValue = req.Authorization
                    }
                },
                lineItems = items.ToArray()
            }
        };

        var response = await Client.CreateTransaction(request);

        return new OneTimePaymentResponse
        {
            AuthCode = response?.transactionResponse?.authCode,
            TransactionId = response?.transactionResponse?.transId
        };
    }
}
