using QuizGame.Data.Models;

namespace QuizGame.Data.Services.DTO.AnswerDTOs;

public class AnswerResponse
{
    public string? Name { get; set; }
    public bool IsCorrect { get; set; }
}

public static class AnswerExtensions
{
    public static AnswerResponse ToAnswerResponse(this Answer answer)
    {
        return new AnswerResponse
        {
            Name = answer.Name,
            IsCorrect = answer.IsCorrect
        };
    }
}