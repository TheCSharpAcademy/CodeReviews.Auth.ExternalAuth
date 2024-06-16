using QuizGame.Data.Models;

namespace QuizGame.Data.Services.DTO.AnswerDTOs;

public class AnswerRequest
{
    public int AnswerId { get; set; }
    public string? Name { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }

    public Answer ToAnswer()
    {
        return new Answer
        {
            AnswerId = AnswerId,
            Name = Name,
            IsCorrect = IsCorrect,
            QuestionId = QuestionId
        };
    }
}