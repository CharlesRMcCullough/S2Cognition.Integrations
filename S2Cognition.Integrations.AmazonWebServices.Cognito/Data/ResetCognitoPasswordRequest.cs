﻿namespace S2Cognition.Integrations.AmazonWebServices.Cognito.Data
{
    public class ResetCognitoPasswordRequest
    {
        public string? UserName { get; set; }
        public string? UserPoolId { get; set; }
    }
}
