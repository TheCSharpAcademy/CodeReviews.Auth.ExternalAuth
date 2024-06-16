using Newtonsoft.Json;

namespace QuizGame.Data.Models.MicrosoftDto;

public class MicrosoftUserInfo
{
    [JsonProperty("email")]
    public string? Email { get; set; }
}