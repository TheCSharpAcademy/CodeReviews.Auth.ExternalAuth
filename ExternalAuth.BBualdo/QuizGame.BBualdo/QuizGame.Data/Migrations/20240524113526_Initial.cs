using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuizGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    QuizId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.QuizId);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Difficulty = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    QuizId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Games_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    QuizId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    AnswerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Quizzes",
                columns: new[] { "QuizId", "Name" },
                values: new object[,]
                {
                    { 1, "General Knowledge" },
                    { 2, "Science" },
                    { 3, "History" },
                    { 4, "Geography" },
                    { 5, "Math" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "QuestionId", "Name", "QuizId" },
                values: new object[,]
                {
                    { 1, "What is the capital of France?", 1 },
                    { 2, "Who wrote 'To Kill a Mockingbird'?", 1 },
                    { 3, "Which planet is known as the Red Planet?", 1 },
                    { 4, "What is the largest mammal in the world?", 1 },
                    { 5, "What is the smallest country in the world?", 1 },
                    { 6, "What is the chemical symbol for water?", 2 },
                    { 7, "Who developed the theory of relativity?", 2 },
                    { 8, "What is the powerhouse of the cell?", 2 },
                    { 9, "What is the hardest natural substance on Earth?", 2 },
                    { 10, "What is the speed of light?", 2 },
                    { 11, "Who was the first President of the United States?", 3 },
                    { 12, "In which year did the Titanic sink?", 3 },
                    { 13, "Who discovered America?", 3 },
                    { 14, "What was the name of the first man on the moon?", 3 },
                    { 15, "Which war was fought between the north and south regions in the United States?", 3 },
                    { 16, "What is the longest river in the world?", 4 },
                    { 17, "Which continent is the largest?", 4 },
                    { 18, "Which country has the most population?", 4 },
                    { 19, "What is the smallest continent?", 4 },
                    { 20, "Which country has the most number of islands?", 4 },
                    { 21, "What is the value of Pi?", 5 },
                    { 22, "What is 2+2?", 5 },
                    { 23, "What is the square root of 16?", 5 },
                    { 24, "What is the value of the gravitational constant?", 5 },
                    { 25, "What is the derivative of x^2?", 5 }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "AnswerId", "IsCorrect", "Name", "QuestionId" },
                values: new object[,]
                {
                    { 1, true, "Paris", 1 },
                    { 2, false, "London", 1 },
                    { 3, false, "Berlin", 1 },
                    { 4, false, "Madrid", 1 },
                    { 5, true, "Harper Lee", 2 },
                    { 6, false, "Jane Austen", 2 },
                    { 7, false, "Mark Twain", 2 },
                    { 8, false, "Ernest Hemingway", 2 },
                    { 9, true, "Mars", 3 },
                    { 10, false, "Venus", 3 },
                    { 11, false, "Jupiter", 3 },
                    { 12, false, "Saturn", 3 },
                    { 13, true, "Blue Whale", 4 },
                    { 14, false, "Elephant", 4 },
                    { 15, false, "Giraffe", 4 },
                    { 16, false, "Whale Shark", 4 },
                    { 17, true, "Vatican City", 5 },
                    { 18, false, "Monaco", 5 },
                    { 19, false, "Nauru", 5 },
                    { 20, false, "San Marino", 5 },
                    { 21, true, "H2O", 6 },
                    { 22, false, "O2", 6 },
                    { 23, false, "CO2", 6 },
                    { 24, false, "N2", 6 },
                    { 25, true, "Albert Einstein", 7 },
                    { 26, false, "Isaac Newton", 7 },
                    { 27, false, "Galileo Galilei", 7 },
                    { 28, false, "Nikola Tesla", 7 },
                    { 29, true, "Mitochondria", 8 },
                    { 30, false, "Nucleus", 8 },
                    { 31, false, "Ribosome", 8 },
                    { 32, false, "Endoplasmic Reticulum", 8 },
                    { 33, true, "Diamond", 9 },
                    { 34, false, "Gold", 9 },
                    { 35, false, "Iron", 9 },
                    { 36, false, "Quartz", 9 },
                    { 37, true, "299,792,458 m/s", 10 },
                    { 38, false, "150,000,000 m/s", 10 },
                    { 39, false, "1,080,000,000 km/h", 10 },
                    { 40, false, "100,000 km/s", 10 },
                    { 41, true, "George Washington", 11 },
                    { 42, false, "Thomas Jefferson", 11 },
                    { 43, false, "Abraham Lincoln", 11 },
                    { 44, false, "John Adams", 11 },
                    { 45, true, "1912", 12 },
                    { 46, false, "1905", 12 },
                    { 47, false, "1898", 12 },
                    { 48, false, "1920", 12 },
                    { 49, true, "Christopher Columbus", 13 },
                    { 50, false, "Ferdinand Magellan", 13 },
                    { 51, false, "Marco Polo", 13 },
                    { 52, false, "James Cook", 13 },
                    { 53, true, "Neil Armstrong", 14 },
                    { 54, false, "Buzz Aldrin", 14 },
                    { 55, false, "Yuri Gagarin", 14 },
                    { 56, false, "Michael Collins", 14 },
                    { 57, true, "American Civil War", 15 },
                    { 58, false, "World War I", 15 },
                    { 59, false, "World War II", 15 },
                    { 60, false, "Spanish-American War", 15 },
                    { 61, true, "Nile River", 16 },
                    { 62, false, "Amazon River", 16 },
                    { 63, false, "Yangtze River", 16 },
                    { 64, false, "Mississippi River", 16 },
                    { 65, true, "Asia", 17 },
                    { 66, false, "Africa", 17 },
                    { 67, false, "North America", 17 },
                    { 68, false, "Europe", 17 },
                    { 69, true, "China", 18 },
                    { 70, false, "India", 18 },
                    { 71, false, "USA", 18 },
                    { 72, false, "Indonesia", 18 },
                    { 73, true, "Australia", 19 },
                    { 74, false, "Europe", 19 },
                    { 75, false, "South America", 19 },
                    { 76, false, "Antarctica", 19 },
                    { 77, true, "Sweden", 20 },
                    { 78, false, "Finland", 20 },
                    { 79, false, "Norway", 20 },
                    { 80, false, "Canada", 20 },
                    { 81, true, "3.14", 21 },
                    { 82, false, "2.71", 21 },
                    { 83, false, "1.62", 21 },
                    { 84, false, "0", 21 },
                    { 85, true, "4", 22 },
                    { 86, false, "3", 22 },
                    { 87, false, "5", 22 },
                    { 88, false, "6", 22 },
                    { 89, true, "4", 23 },
                    { 90, false, "5", 23 },
                    { 91, false, "6", 23 },
                    { 92, false, "7", 23 },
                    { 93, true, "9.8 m/s^2", 24 },
                    { 94, false, "3.14 m/s^2", 24 },
                    { 95, false, "2.71 m/s^2", 24 },
                    { 96, false, "1.62 m/s^2", 24 },
                    { 97, true, "2x", 25 },
                    { 98, false, "x^2", 25 },
                    { 99, false, "1/x", 25 },
                    { 100, false, "x", 25 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_QuizId",
                table: "Games",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Quizzes");
        }
    }
}
