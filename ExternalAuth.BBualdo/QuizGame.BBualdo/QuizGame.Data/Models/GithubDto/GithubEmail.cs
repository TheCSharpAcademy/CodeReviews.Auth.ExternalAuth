using Newtonsoft.Json;

namespace QuizGame.Data.Models.GithubDto;

public class GithubEmail
{
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("primary")]
    public bool Primary { get; set; }

    [JsonProperty("verified")]
    public bool Verified { get; set; }

    [JsonProperty("visibility")]
    public string Visibility { get; set; }
}