using Microsoft.EntityFrameworkCore;
using QuizGame.Data.Models;

namespace QuizGame.Data.Repositories;

public class GamesRepository(QuizGameContext context) : Repository<Game>(context), IGamesRepository
{
    private readonly QuizGameContext _context = context;
    
    public async Task<IEnumerable<Game>> GetGamesWithQuizzesAsync()
    {
        return await _context.Games.Include(g => g.Quiz).ToListAsync();
    }
}