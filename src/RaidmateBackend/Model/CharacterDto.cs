
using System.Text.Json.Serialization;

namespace RaidmateBackend.Model;

public record CharacterDto(
	[property: JsonPropertyName("id")] int Id,
	[property: JsonPropertyName("name")] string Name,
	[property: JsonPropertyName("realm")] RealmDto Realm,
	[property: JsonPropertyName("playable_class")] ClassDto Class,
	[property: JsonPropertyName("playable_race")] RaceDto Race,
	[property: JsonPropertyName("faction")] FactionDto Faction,
	[property: JsonPropertyName("level")] int Level);
