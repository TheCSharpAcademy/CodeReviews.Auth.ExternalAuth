using QuizGame.Data.Models;
using QuizGame.Data.Repositories;
using QuizGame.Data.Services.DTO.GameDTOs;

namespace QuizGame.Data.Services;

public class GamesService(IGamesRepository gamesRepository) : IGamesService
{
    private readonly IGamesRepository _gamesRepository = gamesRepository;

    public async Task AddGameAsync(GameRequest gameRequest)
    {
        await _gamesRepository.AddAsync(gameRequest.ToGame());
    }

    public async Task<bool> DeleteAllGamesAsync()
    {
        IEnumerable<Game> games = await _gamesRepository.GetAsync();
        if (!games.Any()) return false;
        await _gamesRepository.DeleteAllAsync(games);
        return true;
    }

    public async Task<bool> DeleteGameAsync(int id)
    {
        Game? game = await _gamesRepository.GetByIdAsync(id);
        if (game == null) return false;
        await _gamesRepository.DeleteAsync(game);
        return true;
    }

    public async Task<PaginatedGames> GetGamesAsync(int page, int pageSize)
    {
        IEnumerable<Game> games = await _gamesRepository.GetGamesWithQuizzesAsync();
        int totalPages = (int)Math.Ceiling((decimal)games.Count() / pageSize);
        List<Game> paginatedGames = games
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .OrderByDescending(game => game.Score).ToList();

        return new PaginatedGames
        {
            Total = totalPages,
            Games = paginatedGames.Select(game => game.ToGameResponse())
        };
    }
}