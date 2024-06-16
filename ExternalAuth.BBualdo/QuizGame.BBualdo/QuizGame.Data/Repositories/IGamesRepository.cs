using QuizGame.Data.Models;

namespace QuizGame.Data.Repositories;

public interface IGamesRepository : IRepository<Game>
{
    Task<IEnumerable<Game>> GetGamesWithQuizzesAsync();
}