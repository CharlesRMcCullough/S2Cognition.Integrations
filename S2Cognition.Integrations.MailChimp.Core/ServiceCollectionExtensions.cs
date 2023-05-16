using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.Core;
using S2Cognition.Integrations.MailChimp.Core.Models;


namespace S2Cognition.Integrations.MailChimp.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMailChimpIntegration(this IServiceCollection sc)
    {
        return sc.AddIntegrationUtilities()
    .AddScoped<IMailChimpIntegration>(_ => new MailChimpIntegration(_))
    .AddScoped<IMailChimpNativeClient>(_ => new MailChimpNativeClient(_));
    }

}
