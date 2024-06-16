using QuizGame.Data.Models;
using QuizGame.Data.Services.DTO.QuizDTOs;

namespace QuizGame.Data.Repositories;

public interface IQuizzesRepository : IRepository<Quiz>
{
    Task<QuizDetailsResponse?> GetQuizDetails(int id);
}