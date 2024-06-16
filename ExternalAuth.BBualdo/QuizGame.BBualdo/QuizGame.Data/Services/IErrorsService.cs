using QuizGame.Data.Models;

namespace QuizGame.Data.Services;

public interface IErrorsService
{
    public void AddError(Error error);
}