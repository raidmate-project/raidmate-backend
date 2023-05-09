// -------------------------------------------------------------------
//  Copyright (c) Axis Communications AB, SWEDEN. All rights reserved.
// -------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace RaidmateBackend.Model;

public class AccessTokenDto
{
	[JsonPropertyName("access_token")]
	public string AccessToken { get; init; }

	[JsonPropertyName("token_type")]
	public string TokenType { get; init; }

	[JsonPropertyName("expires_in")]
	public int ExpiresIn { get; set; }

	[JsonPropertyName("scope")]
	public string Scope { get; set; }
}
