namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data
{
    public class ListCognitoUsersRequest
    {
        public List<string>? AttributesToGet { get; set; }
        public int Limit { get; set; }
        public string? UserPoolId { get; set; }
        public string? Filter { get; set; }
    }
}
