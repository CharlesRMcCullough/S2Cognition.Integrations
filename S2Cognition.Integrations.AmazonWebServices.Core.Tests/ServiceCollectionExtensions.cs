﻿using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.AmazonWebServices.Core.Data;
using S2Cognition.Integrations.AmazonWebServices.Core.Tests.Fakes;

namespace S2Cognition.Integrations.AmazonWebServices.Core.Tests;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFakeAmazonWebServices(this IServiceCollection sc)
    {
        sc.AddSingleton<IAwsRegionFactory, FakeAwsRegionFactory>()
            .AddScoped<IAwsRegionEndpoint, FakeAwsRegionEndpoint>();

        return sc;
    }
}

