using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using RaidmateBackend.Model;

namespace RaidmateBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
	private const string AccountProfileSummaryApiEndpoint = "https://eu.api.blizzard.com/profile/user/wow?namespace=profile-eu&locale=en_GB";
	private readonly ILogger<AccountController> logger;
	private readonly IHttpClientFactory httpClientFactory;

	public AccountController(
		ILogger<AccountController> logger,
		IHttpClientFactory httpClientFactory)
	{
		this.logger = logger;
		this.httpClientFactory = httpClientFactory;
	}

	/// <summary>
	/// Gets the account summary for the user associated with the access token.
	/// </summary>
	[HttpGet("summary")]
	public async Task<ActionResult<AccountProfileSummaryDto>> GetAccountSummaryAsync()
	{
		var authHeader = Request.Headers.Authorization.FirstOrDefault();
		if (string.IsNullOrEmpty(authHeader))
		{
			return new UnauthorizedResult();
		}

		using var httpClient = httpClientFactory.CreateClient();
		var request = new HttpRequestMessage(HttpMethod.Get, AccountProfileSummaryApiEndpoint);
		if (!AuthenticationHeaderValue.TryParse(Request.Headers.Authorization, out var authenticationHeader))
		{
			return new BadRequestResult();
		}

		request.Headers.Authorization = authenticationHeader;

		var response = await httpClient.SendAsync(request);

		if (!response.IsSuccessStatusCode)
		{
			logger.LogWarning("Invalid response. Got status code {StatusCode} from Blizzard API. Content: {ResponseContent}", response.StatusCode, await response.Content.ReadAsStringAsync());
			return new StatusCodeResult((int)response.StatusCode);
		}

		var account = await response.Content.ReadFromJsonAsync<AccountProfileSummaryDto>() ?? throw new InvalidCastException($"Unable to parse response from Blizzard API. Response: {await response.Content.ReadAsStringAsync()}");

		return account;
	}
}
