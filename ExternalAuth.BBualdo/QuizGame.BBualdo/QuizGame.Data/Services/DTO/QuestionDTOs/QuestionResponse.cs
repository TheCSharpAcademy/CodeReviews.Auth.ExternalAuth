using QuizGame.Data.Models;
using QuizGame.Data.Services.DTO.AnswerDTOs;

namespace QuizGame.Data.Services.DTO.QuestionDTOs;

public class QuestionResponse
{
    public string? Name { get; set; }
    public ICollection<AnswerResponse> Answers { get; set; } = new List<AnswerResponse>();
}

public static class QuestionExtensions
{
    public static QuestionResponse ToQuestionResponse(this Question question)
    {
        return new QuestionResponse
        {
            Name = question.Name,
            Answers = question.Answers.Select(answer => answer.ToAnswerResponse()).ToList()
        };
    }
}