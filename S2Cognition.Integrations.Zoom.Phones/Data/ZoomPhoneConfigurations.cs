using S2Cognition.Integrations.Core.Data;

namespace S2Cognition.Integrations.Zoom.Phones.Data
{
    public class ZoomPhoneConfiguration : Configuration
    {
        public string AccountId { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;

        public ZoomPhoneConfiguration(IServiceProvider ioc)
            : base(ioc)
        {
        }
    }
}
