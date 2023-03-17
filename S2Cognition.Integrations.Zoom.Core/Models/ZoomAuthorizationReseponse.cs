﻿using System.Text.Json.Serialization;

namespace S2Cognition.Integrations.Zoom.Core.Models;

public class ZoomAuthenticationResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public long? ExpireIn { get; set; }

    [JsonPropertyName("scope")]
    public string? Scope { get; set; }
}