namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data
{
    public class ListCognitoUsersRequest
    {
        public List<string>? AttributesToGet { get; set; } = null;
        public int? Limit { get; set; } = null;
        public string? UserPoolId { get; set; } = null;
        public string? Filter { get; set; } = null;
        public string? PaginationToken { get; set; } = null;
    }
}
