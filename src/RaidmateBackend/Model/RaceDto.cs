using System.Text.Json.Serialization;

namespace RaidmateBackend.Model;

public record RaceDto([property: JsonPropertyName("id")] int Id, [property: JsonPropertyName("name")] string Name);
