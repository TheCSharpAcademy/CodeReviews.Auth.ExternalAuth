using Newtonsoft.Json;

namespace QuizGame.Data.Models;

public class FacebookUserInfo
{
    [JsonProperty("email")]
    public string? Email { get; set; }
}