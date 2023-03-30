namespace S2Cognition.Integrations.AuthorizeNet.Core.Data;

public interface IAuthorizeNetIntegration : IIntegration<AuthorizeNetConfiguration>
{
    Task<OneTimePaymentResponse> OneTimePayment(OneTimePaymentRequest req);
}

public class OneTimePaymentRequest 
{
    public IList<SaleItem> Items { get; set; } = Array.Empty<SaleItem>();
    public string? Authorization { get; set; }
}

public class OneTimePaymentResponse 
{
    public string? AuthCode { get; set; }
    public string? TransactionId { get; set; }
}

public class SaleItem
{
    public string? ItemId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
    public double? UnitPrice { get; set; }
}
