using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizGame.Data.Models;

namespace QuizGame.API.Controllers;

[Route("api/facebook")]
[ApiController]
public class FacebookAuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager) : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IConfiguration _configuration = configuration;
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;

    [HttpPost("sign-in")]
    public async Task<ActionResult> SignIn(AuthCodeDto authCodeDto)
    {
        // Get token from code
        var tokenResponse = await ExchangeCodeForToken(authCodeDto.Code);
        // Get user data
        var userInfo = await GetUserInfo(tokenResponse.AccessToken);
        // Create/Login user
        var user = await _userManager.FindByEmailAsync(userInfo.Email);
        if (user == null)
        {
            user = new User { Email = userInfo.Email, UserName = userInfo.Email };
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);

        // Return token
        var authToken = _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "Default");
        return Ok(new { token = authToken });
    }

    private async Task<TokenResponse> ExchangeCodeForToken(string code)
    {
        var client = _httpClientFactory.CreateClient();
        var clientId = _configuration["Authentication:Facebook:AppId"];
        var clientSecret = _configuration["Authentication:Facebook:AppSecret"];
        var redirectUri = "http://localhost:4200/auth/signin-facebook";
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://graph.facebook.com/v20.0/oauth/access_token?client_id={clientId}&client_secret={clientSecret}&redirect_uri={redirectUri}&code={code}");

        var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
            Console.WriteLine($"Response: {content}");
        }

        return JsonConvert.DeserializeObject<TokenResponse>(content);
    }

    private async Task<FacebookUserInfo> GetUserInfo(string accessToken)
    {
        var client = _httpClientFactory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get,
            $"https://graph.facebook.com/me?access_token={accessToken}&fields=email");

        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<FacebookUserInfo>(content);
    }
}