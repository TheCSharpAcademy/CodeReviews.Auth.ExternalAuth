using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizGame.Data.Models;

namespace QuizGame.API.Controllers;

[Route("api/twitter")]
[ApiController]
public class TwitterAuthController(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    UserManager<User> userManager,
    SignInManager<User> signInManager) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly UserManager<User> _userManager = userManager;

    [HttpPost("sign-in")]
    public async Task<ActionResult> SignIn(TwitterAuthCodeDto authCodeDto)
    {
        // Exchanging code from Google auth callback for access token
        var tokenResponse = await ExchangeCodeForToken(authCodeDto.Code, authCodeDto.CodeVerifier);

        // Getting user info
        var userInfo = await GetUserInfo(tokenResponse.AccessToken);

        // Creating user if not exists or logging in
        var user = await _userManager.FindByIdAsync(userInfo.Data.Id);

        if (user == null)
        {
            var randomUsernameId = Guid.NewGuid().ToString()[..4];
            user = new User { UserName = $"{userInfo.Data.Username}{randomUsernameId}", Email = $"{userInfo.Data.Username}@gmail.com", Id = userInfo.Data.Id};
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);
        }

        await _signInManager.SignInAsync(user, false);

        var authToken = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "Default");
        return Ok(new { token = authToken });
    }

    private async Task<TokenResponse> ExchangeCodeForToken(string code, string? codeVerifier)
    {
        var client = _httpClientFactory.CreateClient();

        var clientId = _configuration["Authentication:Twitter:ClientId"];
        var clientSecret = _configuration["Authentication:Twitter:ClientSecret"];
        var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/2/oauth2/token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "grant_type", "authorization_code" },
            { "code", code },
            { "redirect_uri", "http://localhost:4200/auth/signin-twitter" },
            { "code_verifier", codeVerifier }
        });

        var response = await client.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
            Console.WriteLine($"Response: {content}");
        }

        return JsonConvert.DeserializeObject<TokenResponse>(content);
    }

    private async Task<TwitterUserInfo> GetUserInfo(string accessToken)
    {
        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.twitter.com/2/users/me");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
            Console.WriteLine($"Response: {content}");
        }

        return JsonConvert.DeserializeObject<TwitterUserInfo>(content);
    }
}