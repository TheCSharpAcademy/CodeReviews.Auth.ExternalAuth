using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace QuizGame.Data.Models;

public class GoogleUserInfo
{
    [JsonProperty("sub")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Username { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }
}