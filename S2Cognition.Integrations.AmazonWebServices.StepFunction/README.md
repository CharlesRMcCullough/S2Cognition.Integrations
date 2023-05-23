#  S2Cognition.Integrations.AmazonWebServices.StepFunction

Subproject of [S2Cognition.Integrations](../readme.md)

## QuickStart Example

1. Include the necessary NuGet packages into your project: `S2Cognition.Integrations.AmazonWebServices.StepFunction`
2. Initialize your IoC container: `serviceCollection.AddAmazonWebServicesStepFunctionIntegration();`
3. Create the appropriate configuration object:
    ```
    var configuration = new AmazonWebServicesStepFunctionConfiguration(serviceProvider)
    {
    };
    ```
4. Initialize the integration:
    ```
    var integration = serviceProvider.GetRequiredService<IAmazonWebServicesStepFunctionIntegration>();
    await integration.Initialize(configuration);
    ```
5. Call the integration Api's: `await integration.Execute(new ExecuteRequest{...});`

## Public objects

* Configuration Object: `AmazonWebServicesStepFunctionConfiguration`
* Integration Service: `IAmazonWebServicesStepFunctionIntegration`

## Api's

* `async Task<ExecuteResponse> Execute(ExecuteRequest req)`