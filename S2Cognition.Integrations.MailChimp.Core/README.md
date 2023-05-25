#  S2Cognition.Integrations.MailChimp

Subproject of [S2Cognition.Integrations](../readme.md)

## QuickStart Example

1. Include the necessary NuGet packages into your project: `S2Cognition.Integrations.MailChimp`
2. Initialize your IoC container: `serviceCollection.AddMailChimpIntegration();`
3. Create the appropriate configuration object:
    ```
var configuration = new MailChimpConfiguration(serviceProvider)
{
    AccountId = "",
    ClientId = "",
    ClientSecret = ""
};
    ```
4. Initialize the integration:
    ```
    var integration = serviceProvider.GetRequiredService<IMailChimpIntegration>();
    await integration.Initialize(configuration);
    ```
5. Call the integration Api's: `await await integration.GetLists();`

## Public objects

* Configuration Object: `MailChimpConfiguration`
* Integration Service: `IMailChimpIntegration`

## Api's

* `Task<GetListsResponse>? GetLists();`
* `Task<AddUpdateMemberResponse>? MailChimpAddUpdateMember(AddUpdateMemberRequest req)`
