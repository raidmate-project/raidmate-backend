using System.Text.Json.Serialization;

namespace RaidmateBackend.Model;

public record AccessTokenDto(
	[property: JsonPropertyName("access_token")] string AccessToken,
	[property: JsonPropertyName("token_type")] string TokenType,
	[property: JsonPropertyName("expires_in")] int ExpiresIn,
	[property: JsonPropertyName("scope")] string Scope)
{
}
