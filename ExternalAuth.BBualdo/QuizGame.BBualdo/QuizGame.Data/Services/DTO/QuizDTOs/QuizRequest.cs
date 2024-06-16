using QuizGame.Data.Models;
using QuizGame.Data.Services.DTO.QuestionDTOs;

namespace QuizGame.Data.Services.DTO.QuizDTOs;

public class QuizRequest
{
    public int QuizId { get; set; }
    public string? Name { get; set; }
    public ICollection<QuestionRequest> Questions { get; set; } = new List<QuestionRequest>();

    public Quiz ToQuiz()
    {
        return new Quiz
        {
            QuizId = QuizId,
            Name = Name,
            Questions = Questions.Select(question => question.ToQuestion()).ToList()
        };
    }
}