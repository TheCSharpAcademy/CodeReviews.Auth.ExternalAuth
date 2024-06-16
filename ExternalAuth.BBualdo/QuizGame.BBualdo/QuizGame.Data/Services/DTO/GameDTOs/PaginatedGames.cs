namespace QuizGame.Data.Services.DTO.GameDTOs;

public class PaginatedGames
{
    public int Total { get; set; }
    public IEnumerable<GameResponse> Games { get; set; }
}