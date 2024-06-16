using QuizGame.Data.Models;
using QuizGame.Data.Services.DTO.QuestionDTOs;

namespace QuizGame.Data.Services.DTO.QuizDTOs;

public class QuizDetailsResponse
{
    public int QuizId { get; set; }

    public string? Name { get; set; }

    public ICollection<QuestionResponse> Questions { get; set; } = new List<QuestionResponse>();
}

public static partial class QuizExtensions
{
    public static QuizDetailsResponse ToQuizDetailsResponse(this Quiz quiz)
    {
        return new QuizDetailsResponse
        {
            QuizId = quiz.QuizId,
            Name = quiz.Name,
            Questions = quiz.Questions.Select(question => question.ToQuestionResponse()).ToList()
        };
    }
}