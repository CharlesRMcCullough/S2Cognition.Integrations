using S2Cognition.Integrations.AuthorizeNet.Core.Models;

namespace S2Cognition.Integrations.AuthorizeNet.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthorizeNetIntegration(this IServiceCollection sc)
    {
        return sc.AddSingleton<IAuthorizeNetIntegration>(_ => new AuthorizeNetIntegration(_))
            .AddSingleton<IAuthorizeNetClient>(_ => new AuthorizeNetClient());
    }
}
