using Newtonsoft.Json;

namespace QuizGame.Data.Models;

public class TwitterUserInfo
{
    [JsonProperty("data")]
    public TwitterUserData Data { get; set; }
}

public class TwitterUserData
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }
}