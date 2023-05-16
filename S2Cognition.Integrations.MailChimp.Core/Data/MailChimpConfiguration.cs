using S2Cognition.Integrations.Core.Data;

namespace S2Cognition.Integrations.MailChimp.Core.Data;

public class MailChimpConfiguration : Configuration
{

    public string AccountId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;

    public MailChimpConfiguration(IServiceProvider ioc)
    : base(ioc)
    {
    }
}
