using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.Core.Tests;
using FakeItEasy;
using S2Cognition.Integrations.AuthorizeNet.Core.Models;
using S2Cognition.Integrations.AuthorizeNet.Core.Data;
using AuthorizeNet.Api.Contracts.V1;
using Shouldly;

namespace S2Cognition.Integrations.AuthorizeNet.Core.Tests;

public class AuthorizeNetIntegrationTests : UnitTestBase
{
    private IAuthorizeNetClient _client = default!;
    private AuthorizeNetConfiguration _configuration = default!;
    private IAuthorizeNetIntegration _sut = default!;

    protected override async Task IocSetup(IServiceCollection sc)
    {
        sc.AddAuthorizeNetIntegration();

        _client = A.Fake<IAuthorizeNetClient>();
        sc.AddSingleton(_client);

        await Task.CompletedTask;
    }

    protected override async Task TestSetup()
    {
        _configuration = new AuthorizeNetConfiguration(_ioc)
        {
            LoginId = Guid.NewGuid().ToString(),
            TransactionKey = Guid.NewGuid().ToString(),
            Sandbox = true
        };

        _sut = _ioc.GetRequiredService<IAuthorizeNetIntegration>();
        await _sut.Initialize(_configuration);
    }

    [Fact]
    public void EnsureClientIsInitialized()
    {
        A.CallTo(() => _client.Initialize(A<AuthorizeNetConfiguration>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task EnsureOneTimePaymentExecutesAsExpected()
    {
        var clientResponse = new createTransactionResponse
        { 
            transactionResponse = new transactionResponse
            { 
                authCode = Guid.NewGuid().ToString(),
                transId = Guid.NewGuid().ToString()
            }
        };

        var item1 = new SaleItem { Description = Guid.NewGuid().ToString(), ItemId = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString(), Quantity = 123, UnitPrice = 1.23 };
        var item2 = new SaleItem { Description = Guid.NewGuid().ToString(), ItemId = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString(), Quantity = 987, UnitPrice = 9.87 };

        var req = new OneTimePaymentRequest
        {
            Authorization = Guid.NewGuid().ToString(),
            Items = new List<SaleItem> { item1, item2 }
        };

        A.CallTo(() => _client.CreateTransaction(A<createTransactionRequest>._))
            .ReturnsLazily(async (_) => {
                _.Arguments.Count.ShouldBe(1);

                var arg = _.Arguments.Single() as createTransactionRequest;
                arg.ShouldNotBeNull();

                arg.transactionRequest.ShouldNotBeNull();
                arg.transactionRequest.transactionType.ShouldBe(transactionTypeEnum.authCaptureTransaction.ToString());
                arg.transactionRequest.amount.ShouldBe((decimal)((item1.UnitPrice * item1.Quantity) + (item2.UnitPrice * item2.Quantity)));
                arg.transactionRequest.amountSpecified.ShouldBeTrue();
                arg.transactionRequest.payment.ShouldNotBeNull();
                arg.transactionRequest.payment.Item.ShouldNotBeNull();
                
                var payment = arg.transactionRequest.payment.Item.ShouldBeAssignableTo<opaqueDataType>();
                payment.ShouldNotBeNull();
                payment.dataDescriptor.ShouldBe(AuthorizeNetIntegration.HostedFormTokenIdentifier);
                payment.dataValue.ShouldBe(req.Authorization);
                
                arg.transactionRequest.lineItems.ShouldNotBeNull();
                arg.transactionRequest.lineItems.Length.ShouldBe(2);
                var firstItem = arg.transactionRequest.lineItems[0];
                var secondItem = arg.transactionRequest.lineItems[1];
                var firstExpectation = item1;
                var secondExpectation = item2;
                if (firstItem.itemId == item2.ItemId)
                {
                    firstExpectation = item2;
                    secondExpectation = item1;
                }

                firstItem.itemId.ShouldBe(firstExpectation.ItemId);
                firstItem.name.ShouldBe(firstExpectation.Name);
                firstItem.description.ShouldBe(firstExpectation.Description);
                firstItem.quantity.ShouldBe((decimal)firstExpectation.Quantity);
                firstItem.unitPrice.ShouldBe((decimal)firstExpectation.UnitPrice);
                secondItem.itemId.ShouldBe(secondExpectation.ItemId);
                secondItem.name.ShouldBe(secondExpectation.Name);
                secondItem.description.ShouldBe(secondExpectation.Description);
                secondItem.quantity.ShouldBe((decimal)secondExpectation.Quantity);
                secondItem.unitPrice.ShouldBe((decimal)secondExpectation.UnitPrice);

                return await Task.FromResult(clientResponse);
            });

        var resp = await _sut.OneTimePayment(req);

        A.CallTo(() => _client.CreateTransaction(A<createTransactionRequest>._)).MustHaveHappenedOnceExactly();
        resp.ShouldNotBeNull();
        resp.AuthCode.ShouldBe(clientResponse.transactionResponse.authCode);
        resp.TransactionId.ShouldBe(clientResponse.transactionResponse.transId);
    }
}