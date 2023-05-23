using Amazon.StepFunctions.Model;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.AmazonWebServices.StepFunction.Data;
using S2Cognition.Integrations.AmazonWebServices.StepFunction.Models;
using S2Cognition.Integrations.Core.Tests;
using Shouldly;

namespace S2Cognition.Integrations.AmazonWebServices.StepFunction.Tests
{
    public class StepFunctionTests : UnitTestBase
    {
        private IAmazonWebServicesStepFunctionIntegration _sut = default!;
        private IAwsStepFunctionClient _client = default!;

        protected override async Task IocSetup(IServiceCollection sc)
        {
            sc.AddAmazonWebServicesStepFunctionIntegration();
            sc.AddSingleton(_ => A.Fake<IAwsStepFunctionClient>());

            await Task.CompletedTask;
        }

        protected override async Task TestSetup()
        {
            var configuration = new AmazonWebServicesStepFunctionConfiguration(_ioc);

            _sut = _ioc.GetRequiredService<IAmazonWebServicesStepFunctionIntegration>();

            await _sut.Initialize(configuration);

            _client = _ioc.GetRequiredService<IAwsStepFunctionClient>();
        }

        [Fact]
        public async Task EnsureSomething()
        {
            var req = new ExecuteRequest { };
            var resp = await _sut.Execute(req);

            resp.ShouldNotBeNull();
            A.CallTo(() => _client.Execute(A<StartExecutionRequest>._)).MustHaveHappenedOnceExactly();
        }
    }
}
