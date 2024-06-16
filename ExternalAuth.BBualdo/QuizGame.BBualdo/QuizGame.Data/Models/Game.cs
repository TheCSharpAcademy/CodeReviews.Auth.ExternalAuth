using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGame.Data.Models;

public class Game
{
    [Key] public int GameId { get; set; }
    [Required] [StringLength(50)] public string? Username { get; set; }
    [Required] [StringLength(6)] public string? Difficulty { get; set; }
    [Required] public DateTime Date { get; set; }
    [Required] public int Score { get; set; }
    public int QuizId { get; set; }
    [ForeignKey(nameof(QuizId))] public Quiz? Quiz { get; set; }
}