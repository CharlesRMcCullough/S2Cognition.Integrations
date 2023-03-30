namespace S2Cognition.Integrations.AuthorizeNet.Core.Data.HostedForm;

public class AdnOpaqueData
{
    public string? DataDescriptor { get; set; }
    public string? DataValue { get; set; }
}

public class AdnPaymentResponse
{
    public AdnOpaqueData? OpaqueData { get; set; }
    public AdnMessages? Messages { get; set; }
    public AdnEncryptedCardData? EncryptedCardData { get; set; }
    public AdnCustomerInformation? CustomerInformation { get; set; }
}

public class AdnMessages
{
    public string? ResultCode { get; set; }
    public IList<AdnMessage>? Message { get; set; }
}

public class AdnMessage
{
    public string? Code { get; set; }
    public string? Text { get; set; }
}

public class AdnEncryptedCardData
{
    public string? CardNumber { get; set; }
    public string? ExpDate { get; set; }
    public string? Bin { get; set; }
}

public class AdnCustomerInformation
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
