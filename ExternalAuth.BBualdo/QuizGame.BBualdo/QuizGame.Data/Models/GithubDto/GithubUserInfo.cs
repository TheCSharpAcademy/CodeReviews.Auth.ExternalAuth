using Newtonsoft.Json;

namespace QuizGame.Data.Models.Github;

public class GithubUserInfo
{
    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("login")]
    public string? Username { get; set; }
}