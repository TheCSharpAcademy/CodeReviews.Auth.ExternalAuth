using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizGame.Data.Models;

namespace QuizGame.API.Controllers;

[Route("api/Google")]
[ApiController]
public class GoogleAuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager) : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IConfiguration _configuration = configuration;
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;

    [HttpPost("sign-in")]
    public async Task<ActionResult> SignIn(AuthCodeDto authCodeDto)
    {
        // Exchanging code from Google auth callback for access token
        var tokenResponse = await ExchangeCodeForToken(authCodeDto.Code);

        // Getting user info
        var userInfo = await GetUserInfo(tokenResponse.AccessToken);

        // Creating user if not exists or logging in
        var user = await _userManager.FindByEmailAsync(userInfo.Email);

        if (user == null)
        {
            user = new User() { UserName = userInfo.Username.Replace(" ", ""), Email = userInfo.Email };
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);
        }

        await _signInManager.SignInAsync(user, isPersistent:false);

        var authToken = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "Default");
        return Ok(new { token = authToken });
    }

    private async Task<TokenResponse> ExchangeCodeForToken(string code)
    {
        var client = _httpClientFactory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Post, "https://www.googleapis.com/oauth2/v4/token");
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            {"client_id", _configuration["Authentication:Google:ClientId"]},
            {"client_secret", _configuration["Authentication:Google:ClientSecret"]},
            {"grant_type", "authorization_code"},
            {"code", code},
            {"redirect_uri", "http://localhost:4200/auth/signin-google"},
        });

        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TokenResponse>(content);
    }

    private async Task<GoogleUserInfo> GetUserInfo(string accessToken)
    {
        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/oauth2/v2/userinfo");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<GoogleUserInfo>(content);
    }
}