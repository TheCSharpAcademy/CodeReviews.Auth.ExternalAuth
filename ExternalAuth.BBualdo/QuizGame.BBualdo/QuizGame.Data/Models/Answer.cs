using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGame.Data.Models;

public class Answer
{
    [Key] public int AnswerId { get; set; }
    [Required] [StringLength(50)] public string? Name { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
    [ForeignKey(nameof(QuestionId))] public Question? Question { get; set; }
}