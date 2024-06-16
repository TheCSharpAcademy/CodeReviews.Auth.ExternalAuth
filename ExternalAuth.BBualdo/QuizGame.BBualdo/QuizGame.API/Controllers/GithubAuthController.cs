using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizGame.Data.Models;
using QuizGame.Data.Models.Github;
using QuizGame.Data.Models.GithubDto;

namespace QuizGame.API.Controllers;

[Route("api/github")]
[ApiController]
public class GithubAuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager) : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IConfiguration _configuration = configuration;
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;

    [HttpPost("sign-in")]
    public async Task<ActionResult> SignIn(AuthCodeDto authCodeDto)
    {
        var tokenResponse = await ExchangeCodeForToken(authCodeDto.Code);
        var userInfo = await GetUserInfo(tokenResponse.AccessToken);
        var email = userInfo.Email;
        if (string.IsNullOrEmpty(email))
        {
            email = await GetUserEmail(tokenResponse.AccessToken);
        }


        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new User
            {
                Email = email,
                UserName = userInfo.Username
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);

        var authToken = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "Default");
        return Ok(new { token = authToken });
    }

    private async Task<TokenResponse> ExchangeCodeForToken(string code)
    {
        var client = _httpClientFactory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            {"client_id", _configuration["Authentication:Github:ClientId"]},
            {"client_secret", _configuration["Authentication:Github:ClientSecret"]},
            {"code", code},
            {"redirect_uri", "http://localhost:4200/auth/signin-github"},
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

    private async Task<GithubUserInfo> GetUserInfo(string accessToken)
    {
        var client = _httpClientFactory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.UserAgent.ParseAdd("QuizGame");
        var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
            Console.WriteLine($"Response: {content}");
        }

        return JsonConvert.DeserializeObject<GithubUserInfo>(content);
    }

    private async Task<string> GetUserEmail(string accessToken)
    {
        var client = _httpClientFactory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user/emails");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.UserAgent.ParseAdd("QuizGame");
        var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
            Console.WriteLine($"Response: {content}");
        }

        var emails = JsonConvert.DeserializeObject<List<GithubEmail>>(content);
        var primaryEmail = emails.FirstOrDefault(e => e.Primary).Email;
        return primaryEmail;
    }
}