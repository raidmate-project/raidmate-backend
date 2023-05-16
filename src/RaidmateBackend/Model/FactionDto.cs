using System.Text.Json.Serialization;

namespace RaidmateBackend.Model;

public record FactionDto([property: JsonPropertyName("name")] string Name);
