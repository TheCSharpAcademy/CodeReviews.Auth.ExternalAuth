using QuizGame.Data.Models;

namespace QuizGame.Data.Services.DTO.QuizDTOs;

public class QuizResponse
{
    public int QuizId { get; set; }
    public string? Name { get; set; }
}

public static partial class QuizExtensions
{
    public static QuizResponse ToQuizResponse(this Quiz quiz)
    {
        return new QuizResponse
        {
            QuizId = quiz.QuizId,
            Name = quiz.Name
        };
    }
}