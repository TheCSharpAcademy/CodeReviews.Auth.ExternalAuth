using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizGame.Data.Dummies;
using QuizGame.Data.Models;

namespace QuizGame.Data;

public class QuizGameContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Error> Errors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Seed();
    }
}

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quiz>().HasData(DummyData.GetQuizzes());
        modelBuilder.Entity<Question>().HasData(DummyData.GetQuestions());
        modelBuilder.Entity<Answer>().HasData(DummyData.GetAnswers());
    }
}