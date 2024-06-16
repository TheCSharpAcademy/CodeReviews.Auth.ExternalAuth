using Newtonsoft.Json;

namespace QuizGame.Data.Models;

public class TokenResponse
{
    [JsonProperty("access_token")]
    public string? AccessToken { get; set; }
}