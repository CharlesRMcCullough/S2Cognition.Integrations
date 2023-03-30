namespace S2Cognition.Integrations.AuthorizeNet.Core.Data
{
    public class AuthorizeNetConfiguration : Configuration
    {
        public string? LoginId { get; set; } 
        public string? TransactionKey { get; set; } 
        public bool? Sandbox { get; set; } 

        public AuthorizeNetConfiguration(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
    }
}
