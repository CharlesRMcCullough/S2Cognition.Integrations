using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.Core.Tests;
using S2Cognition.Integrations.Zoom.Phones.Data;
using Shouldly;

namespace S2Cognition.Integrations.Zoom.Phones.Tests
{
    public class IntegrationTests : UnitTestBase
    {
        public ZoomPhoneIntegration _sut = default!;
        public ZoomPhoneConfiguration _configuration = default!;
        public IZoomPhoneNativeClient _client = default!;

        protected override async Task IocSetup(IServiceCollection sc)
        {
            sc.AddZoomPhoneIntegration();

            sc.AddSingleton<IZoomPhoneIntegration>(_ =>
            {
                var sut = A.Fake<ZoomPhoneIntegration>(__ => __.WithArgumentsForConstructor(new object?[] { _ }));
                A.CallTo(sut).CallsBaseMethod();
                return sut;
            });
            _client = A.Fake<IZoomPhoneNativeClient>();
            sc.AddSingleton(_client);

            await Task.CompletedTask;
        }

        protected override async Task TestSetup()
        {
            _configuration = new ZoomPhoneConfiguration(_ioc)
            {
                AccountId = "fake account id",
                ClientId = "fake client id",
                ClientSecret = "fake client secret"
            };

            var zoomPhoneIntegration = _ioc.GetRequiredService<IZoomPhoneIntegration>() as ZoomPhoneIntegration;
            if (zoomPhoneIntegration == null)
                throw new InvalidOperationException();

            _sut = zoomPhoneIntegration;
            await _sut.Initialize(_configuration);
        }

        [Fact]
        public async Task EnsureConstructionReturnsValidInstance()
        {
            _sut.ShouldNotBeNull();
            (await _sut.IsInitialized()).ShouldBeTrue();
        }
    }
}
