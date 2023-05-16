using System.Text.Json.Serialization;

namespace RaidmateBackend.Model;

public record AccountProfileSummaryDto([property: JsonPropertyName("id")] int Id, [property: JsonPropertyName("wow_accounts")] WowAccountDto[] WowAccounts)
{
}
