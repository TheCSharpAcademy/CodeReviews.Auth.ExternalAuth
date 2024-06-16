using QuizGame.Data.Models;

namespace QuizGame.Data.Services.DTO.GameDTOs;

public class GameRequest
{
    public int GameId { get; set; }
    public string? Username { get; set; }
    public string? Difficulty { get; set; }
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public int QuizId { get; set; }

    public Game ToGame()
    {
        return new Game
        {
            GameId = GameId,
            Username = Username,
            Difficulty = Difficulty,
            Date = Date,
            Score = Score,
            QuizId = QuizId
        };
    }
}