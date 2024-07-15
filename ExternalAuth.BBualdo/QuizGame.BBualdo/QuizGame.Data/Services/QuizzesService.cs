using QuizGame.Data.Models;
using QuizGame.Data.Repositories;
using QuizGame.Data.Services.DTO.QuizDTOs;

namespace QuizGame.Data.Services;

public class QuizzesService(IQuizzesRepository quizzesRepository) : IQuizzesService
{
    private readonly IQuizzesRepository _quizzesRepository = quizzesRepository;


    public async Task<IEnumerable<QuizResponse>> GetQuizzesAsync()
    {
        var quizzes = await _quizzesRepository.GetAsync();
        return quizzes.Select(quiz => quiz.ToQuizResponse());
    }

    public async Task<QuizDetailsResponse?> GetQuizByIdAsync(int id)
    {
        return await _quizzesRepository.GetQuizDetails(id);
    }

    public async Task AddQuizAsync(QuizRequest quizRequest)
    {
        await _quizzesRepository.AddAsync(quizRequest.ToQuiz());
    }

    public async Task<bool> DeleteQuizAsync(int id)
    {
        Quiz? quiz = await _quizzesRepository.GetByIdAsync(id);
        if (quiz == null) return false;
        await _quizzesRepository.DeleteAsync(quiz);
        return true;
    }
}