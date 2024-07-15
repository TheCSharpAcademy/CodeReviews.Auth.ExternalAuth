using QuizGame.Data.Services.DTO;
using QuizGame.Data.Services.DTO.GameDTOs;

namespace QuizGame.Data.Services;

public interface IGamesService
{
    Task<PaginatedGames> GetGamesAsync(int page, int pageSize);
    Task AddGameAsync(GameRequest gameRequest);
    Task<bool> DeleteGameAsync(int id);
    Task<bool> DeleteAllGamesAsync();
}