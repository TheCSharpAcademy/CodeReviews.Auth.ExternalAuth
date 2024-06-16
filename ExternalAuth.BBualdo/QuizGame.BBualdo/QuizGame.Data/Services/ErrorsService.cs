using QuizGame.Data.Models;

namespace QuizGame.Data.Services;

public class ErrorsService(QuizGameContext quizGameContext) : IErrorsService
{
    private readonly QuizGameContext _quizGameContext = quizGameContext;

    public void AddError(Error error)
    {
        _quizGameContext.Errors.Add(error);
        _quizGameContext.SaveChanges();
    }
}