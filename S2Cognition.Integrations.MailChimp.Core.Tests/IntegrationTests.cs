using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.Core.Tests;
using S2Cognition.Integrations.MailChimp.Core.Data;
using S2Cognition.Integrations.MailChimp.Core.Models;
using Shouldly;

namespace S2Cognition.Integrations.MailChimp.Core.Tests;

public class IntegrationTests : UnitTestBase
{
    public MailChimpIntegration _sut = default!;
    public MailChimpConfiguration _configuration = default!;
    public IMailChimpNativeClient _client = default!;

    protected override async Task IocSetup(IServiceCollection sc)
    {
        sc.AddMailChimpIntegration();

        sc.AddSingleton<IMailChimpIntegration>(_ =>
        {
            var sut = A.Fake<MailChimpIntegration>(__ => __.WithArgumentsForConstructor(new object?[] { _ }));
            A.CallTo(sut).CallsBaseMethod();
            return sut;
        });
        _client = A.Fake<IMailChimpNativeClient>();
        sc.AddSingleton(_client);

        await Task.CompletedTask;
    }

    protected override async Task TestSetup()
    {
        _configuration = new MailChimpConfiguration(_ioc)
        {
            AccountId = "fake account id",
            ClientId = "fake client id",
            ClientSecret = "fake client secret"
        };

        var mailChimpIntegration = _ioc.GetRequiredService<IMailChimpIntegration>() as MailChimpIntegration;
        if (mailChimpIntegration == null)
            throw new InvalidOperationException();

        _sut = mailChimpIntegration;
        await _sut.Initialize(_configuration);
    }


    [Fact]
    public async Task EnsureConstructionReturnsValidInstance()
    {
        _sut.ShouldNotBeNull();
        (await _sut.IsInitialized()).ShouldBeTrue();
    }

}
