using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGame.Data.Models;

public class Question
{
    [Key] public int QuestionId { get; set; }
    [Required] [StringLength(200)] public string? Name { get; set; }
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    public int QuizId { get; set; }
    [ForeignKey(nameof(QuizId))] public Quiz? Quiz { get; set; }
}