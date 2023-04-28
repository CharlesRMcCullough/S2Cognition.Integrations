namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data
{
    public class ResetCognitoPasswordRequest
    {
        public string? UserName { get; set; } = null;
        public string? UserPoolId { get; set; } = null;
    }
}
