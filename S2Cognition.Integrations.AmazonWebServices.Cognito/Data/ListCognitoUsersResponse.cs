namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data;

public class ListCognitoUsersResponse
{
    public IList<CognitoUserRecord>? UserRecords { get; set; }
}

public class CognitoUserRecord
{
    public string? UserStatus { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public string? EmailVerified { get; set; }
    public string? Email { get; set; }
    public DateTime? CreatedDate { get; set; }

}