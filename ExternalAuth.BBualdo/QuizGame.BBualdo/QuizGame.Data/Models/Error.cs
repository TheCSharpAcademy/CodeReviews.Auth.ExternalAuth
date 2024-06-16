using System.ComponentModel.DataAnnotations;

namespace QuizGame.Data.Models;

public class Error
{
    [Key] public int Id { get; set; }
    [Required] public string Message { get; set; }
    [Required] public DateTime Time { get; set; }
}