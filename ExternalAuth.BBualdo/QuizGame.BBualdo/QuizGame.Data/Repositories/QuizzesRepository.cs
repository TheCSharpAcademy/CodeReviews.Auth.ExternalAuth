using Microsoft.EntityFrameworkCore;
using QuizGame.Data.Models;
using QuizGame.Data.Services.DTO.QuizDTOs;

namespace QuizGame.Data.Repositories;

public class QuizzesRepository(QuizGameContext context) : Repository<Quiz>(context), IQuizzesRepository
{
    private readonly QuizGameContext _context = context;

    public async Task<QuizDetailsResponse?> GetQuizDetails(int id)
    {
        Quiz? quiz = await _context.Quizzes.Include(q => q.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(quiz => quiz.QuizId == id);
        return quiz.ToQuizDetailsResponse();
    }
}