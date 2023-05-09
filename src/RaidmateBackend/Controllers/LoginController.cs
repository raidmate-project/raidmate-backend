// -------------------------------------------------------------------
//  Copyright (c) Axis Communications AB, SWEDEN. All rights reserved.
// -------------------------------------------------------------------

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RaidmateBackend.Model;

namespace RaidmateBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
	private readonly ILogger<LoginController> logger;
	private readonly IHttpClientFactory httpClientFactory;
	private readonly IConfiguration configuration;

	public LoginController(
		ILogger<LoginController> logger,
		IHttpClientFactory httpClientFactory,
		IConfiguration configuration)
	{
		this.logger = logger;
		this.httpClientFactory = httpClientFactory;
		this.configuration = configuration;
	}

	/// <summary>
	/// Gets the acces token from the Blizzard API and returns it.
	/// </summary>
	[HttpGet("token")]
	public async Task<ActionResult<AccessTokenDto>> GetTokenAsync()
	{
		if (!Request.Headers.TryGetValue("blizzard-code", out var codeHeaders))
		{
			return new BadRequestResult();
		}

		var code = codeHeaders.First();

		logger.LogInformation("Got code {BlizzardCode}", code);

		var clientId = configuration.GetValue<string>("BLIZZARD_CLIENT_ID");
		var clientSecret = configuration.GetValue<string>("BLIZZARD_CLIENT_SECRET");

		var httpClient = httpClientFactory.CreateClient();
		var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth.battle.net/token");
		request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));
		request.Headers.TryAddWithoutValidation("cache-control", "no-cache");
		request.Headers.TryAddWithoutValidation("content-type", "application/x-www-form-urlencoded");

		var parameters = new Dictionary<string, string>
		{
			{ "scope", "wow.profile" },
			//{ "redirect_uri", "https://localhost:53846/login/redirect" },
			{ "redirect_uri", "http://localhost:8080/blizzard-login" },
			{ "grant_type", "authorization_code" },
			{ "code", code }
		};

		request.Content = new FormUrlEncodedContent(parameters);

		var response = await httpClient.SendAsync(request);

		if (!response.IsSuccessStatusCode)
		{
			logger.LogWarning("Invalid code. Got {ResponseStatusCode} from Blizzard API. Content: {ResponseContent}", response.StatusCode, await response.Content.ReadAsStringAsync());
			return new UnauthorizedResult();
		}

		var accessTokenDto = await response.Content.ReadFromJsonAsync<AccessTokenDto>() ?? throw new InvalidOperationException();

		logger.LogInformation("Returning access token {AccessToken}", JsonSerializer.Serialize(accessTokenDto, new JsonSerializerOptions { WriteIndented = true }));

		return new OkObjectResult(accessTokenDto);
	}
}

