﻿using Microsoft.Extensions.DependencyInjection;
using S2Cognition.Integrations.AmazonWebServices.CloudWatch.Data;
using S2Cognition.Integrations.AmazonWebServices.CloudWatch.Models;

namespace S2Cognition.Integrations.AmazonWebServices.CloudWatch;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAmazonWebServicesCloudWatchIntegration(this IServiceCollection sc)
    {
        return sc.AddSingleton<IAmazonWebServicesCloudWatchIntegration, AmazonWebServicesCloudWatchIntegration>()
            .AddSingleton<IAwsCloudWatchConfigFactory, AwsCloudWatchConfigFactory>()
            .AddSingleton<IAwsCloudWatchClientFactory, AwsCloudWatchClientFactory>();
    }
}



