using System.ComponentModel.DataAnnotations;

namespace QuizGame.Data.Models;

public class Quiz
{
    [Key] public int QuizId { get; set; }
    [Required] [StringLength(50)] public string? Name { get; set; }
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public IEnumerable<Game> Games { get; set; } = new List<Game>();
}