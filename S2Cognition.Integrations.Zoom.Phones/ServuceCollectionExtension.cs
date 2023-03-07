using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.Core;

namespace S2Cognition.Integrations.Zoom.Phones;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZoomPhoneIntegration(this IServiceCollection sc)
    {
        return sc.AddIntegrationUtilities()
            .AddScoped<IZoomPhoneIntegration>(_ => new ZoomPhoneIntegration(_));
    }
}
