using System.Text.Json.Serialization;

namespace RaidmateBackend.Model;

public record WowAccountDto([property: JsonPropertyName("id")] int Id, [property: JsonPropertyName("characters")] List<CharacterDto> Characters);
