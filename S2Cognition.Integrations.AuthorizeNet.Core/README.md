#  S2Cognition.Integrations.AuthorizeNet.Core

Subproject of [S2Cognition.Integrations](../readme.md)

The OneTimePayment call in this project requires pre-authorization.   This preauthorization comes from the Authorize.NET
[Hosted Form](https://developer.authorize.net/api/reference/features/acceptjs.html#Using_the_Hosted_Payment_Information_Form)
which requires a UI, and is therefore out of scope for this project.

However, it's simple to setup and use.  When doing so, use the 
[test credit cards](https://developer.authorize.net/hello_world/testing_guide.html)
in sandbox, and get the authorization from the resulting OpaqueData.

## QuickStart Example

1. Include the necessary NuGet packages into your project: `S2Cognition.Integrations.AuthorizeNet.Core`
2. Initialize your IoC container: `serviceCollection.AddAuthorizeNetIntegration();`
3. Create the appropriate configuration object:
    ```
    var configuration = new AuthorizeNetConfiguration(serviceProvider)
    {
        LoginId = "Your API Login ID",
        TransactionKey = "Your Transaction Key",
        Sandbox = true
    };
    ```
4. Initialize the integration:
    ```
    var integration = serviceProvider.GetRequiredService<IAuthorizeNetIntegration>();
    await integration.Initialize(configuration);
    ```
5. Call the integration Api's: `await sut.OneTimePayment(req);`

## Public objects

* Configuration Object: `AuthorizeNetConfiguration`
* Integration Service: `IAuthorizeNetIntegration`

## Api's

* `Task<OneTimePaymentResponse> OneTimePayment(OneTimePaymentRequest req)`
  * Request Object: `OneTimePaymentRequest`
  * Response Object: `OneTimePaymentResponse`
