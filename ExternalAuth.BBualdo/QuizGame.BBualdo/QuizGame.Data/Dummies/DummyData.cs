using QuizGame.Data.Models;

namespace QuizGame.Data.Dummies;

public static class DummyData
{
    public static List<Quiz> GetQuizzes()
    {
        var quizzes = new List<Quiz>
        {
            new Quiz { QuizId = 1, Name = "General Knowledge" },
            new Quiz { QuizId = 2, Name = "Science" },
            new Quiz { QuizId = 3, Name = "History" },
            new Quiz { QuizId = 4, Name = "Geography" },
            new Quiz { QuizId = 5, Name = "Math" }
        };

        return quizzes;
    }

    public static List<Question> GetQuestions()
    {
        var questions = new List<Question>
        {
            // General Knowledge Questions
            new Question { QuestionId = 1, Name = "What is the capital of France?", QuizId = 1 },
            new Question
                { QuestionId = 2, Name = "Who wrote 'To Kill a Mockingbird'?", QuizId = 1 },
            new Question
                { QuestionId = 3, Name = "Which planet is known as the Red Planet?", QuizId = 1 },
            new Question
            {
                QuestionId = 4, Name = "What is the largest mammal in the world?", QuizId = 1
            },
            new Question
            {
                QuestionId = 5, Name = "What is the smallest country in the world?", QuizId = 1
            },
            // Science Questions
            new Question
                { QuestionId = 6, Name = "What is the chemical symbol for water?", QuizId = 2 },
            new Question
                { QuestionId = 7, Name = "Who developed the theory of relativity?", QuizId = 2 },
            new Question
                { QuestionId = 8, Name = "What is the powerhouse of the cell?", QuizId = 2 },
            new Question
                { QuestionId = 9, Name = "What is the hardest natural substance on Earth?", QuizId = 2 },
            new Question { QuestionId = 10, Name = "What is the speed of light?", QuizId = 2 },
            // History Questions
            new Question
            {
                QuestionId = 11, Name = "Who was the first President of the United States?",
                QuizId = 3
            },
            new Question
                { QuestionId = 12, Name = "In which year did the Titanic sink?", QuizId = 3 },
            new Question { QuestionId = 13, Name = "Who discovered America?", QuizId = 3 },
            new Question
            {
                QuestionId = 14, Name = "What was the name of the first man on the moon?",
                QuizId = 3
            },
            new Question
            {
                QuestionId = 15,
                Name = "Which war was fought between the north and south regions in the United States?",
                QuizId = 3
            },
            // Geography Questions
            new Question
            {
                QuestionId = 16, Name = "What is the longest river in the world?", QuizId = 4
            },
            new Question { QuestionId = 17, Name = "Which continent is the largest?", QuizId = 4 },
            new Question
                { QuestionId = 18, Name = "Which country has the most population?", QuizId = 4 },
            new Question { QuestionId = 19, Name = "What is the smallest continent?", QuizId = 4 },
            new Question
            {
                QuestionId = 20, Name = "Which country has the most number of islands?", QuizId = 4
            },
            // Math Questions
            new Question { QuestionId = 21, Name = "What is the value of Pi?", QuizId = 5 },
            new Question { QuestionId = 22, Name = "What is 2+2?", QuizId = 5 },
            new Question { QuestionId = 23, Name = "What is the square root of 16?", QuizId = 5 },
            new Question
            {
                QuestionId = 24, Name = "What is the value of the gravitational constant?",
                QuizId = 5
            },
            new Question { QuestionId = 25, Name = "What is the derivative of x^2?", QuizId = 5 }
        };

        return questions;
    }

    public static List<Answer> GetAnswers()
    {
        var answers = new List<Answer>
        {
            // Answers for General Knowledge Questions
            new Answer { AnswerId = 1, Name = "Paris", IsCorrect = true, QuestionId = 1 },
            new Answer { AnswerId = 2, Name = "London", IsCorrect = false, QuestionId = 1 },
            new Answer { AnswerId = 3, Name = "Berlin", IsCorrect = false, QuestionId = 1 },
            new Answer { AnswerId = 4, Name = "Madrid", IsCorrect = false, QuestionId = 1 },
            new Answer { AnswerId = 5, Name = "Harper Lee", IsCorrect = true, QuestionId = 2 },
            new Answer { AnswerId = 6, Name = "Jane Austen", IsCorrect = false, QuestionId = 2 },
            new Answer { AnswerId = 7, Name = "Mark Twain", IsCorrect = false, QuestionId = 2 },
            new Answer { AnswerId = 8, Name = "Ernest Hemingway", IsCorrect = false, QuestionId = 2 },
            new Answer { AnswerId = 9, Name = "Mars", IsCorrect = true, QuestionId = 3 },
            new Answer { AnswerId = 10, Name = "Venus", IsCorrect = false, QuestionId = 3 },
            new Answer { AnswerId = 11, Name = "Jupiter", IsCorrect = false, QuestionId = 3 },
            new Answer { AnswerId = 12, Name = "Saturn", IsCorrect = false, QuestionId = 3 },
            new Answer { AnswerId = 13, Name = "Blue Whale", IsCorrect = true, QuestionId = 4 },
            new Answer { AnswerId = 14, Name = "Elephant", IsCorrect = false, QuestionId = 4 },
            new Answer { AnswerId = 15, Name = "Giraffe", IsCorrect = false, QuestionId = 4 },
            new Answer { AnswerId = 16, Name = "Whale Shark", IsCorrect = false, QuestionId = 4 },
            new Answer { AnswerId = 17, Name = "Vatican City", IsCorrect = true, QuestionId = 5 },
            new Answer { AnswerId = 18, Name = "Monaco", IsCorrect = false, QuestionId = 5 },
            new Answer { AnswerId = 19, Name = "Nauru", IsCorrect = false, QuestionId = 5 },
            new Answer { AnswerId = 20, Name = "San Marino", IsCorrect = false, QuestionId = 5 },
            // Answers for Science Questions
            new Answer { AnswerId = 21, Name = "H2O", IsCorrect = true, QuestionId = 6 },
            new Answer { AnswerId = 22, Name = "O2", IsCorrect = false, QuestionId = 6 },
            new Answer { AnswerId = 23, Name = "CO2", IsCorrect = false, QuestionId = 6 },
            new Answer { AnswerId = 24, Name = "N2", IsCorrect = false, QuestionId = 6 },
            new Answer { AnswerId = 25, Name = "Albert Einstein", IsCorrect = true, QuestionId = 7 },
            new Answer { AnswerId = 26, Name = "Isaac Newton", IsCorrect = false, QuestionId = 7 },
            new Answer { AnswerId = 27, Name = "Galileo Galilei", IsCorrect = false, QuestionId = 7 },
            new Answer { AnswerId = 28, Name = "Nikola Tesla", IsCorrect = false, QuestionId = 7 },
            new Answer { AnswerId = 29, Name = "Mitochondria", IsCorrect = true, QuestionId = 8 },
            new Answer { AnswerId = 30, Name = "Nucleus", IsCorrect = false, QuestionId = 8 },
            new Answer { AnswerId = 31, Name = "Ribosome", IsCorrect = false, QuestionId = 8 },
            new Answer { AnswerId = 32, Name = "Endoplasmic Reticulum", IsCorrect = false, QuestionId = 8 },
            new Answer { AnswerId = 33, Name = "Diamond", IsCorrect = true, QuestionId = 9 },
            new Answer { AnswerId = 34, Name = "Gold", IsCorrect = false, QuestionId = 9 },
            new Answer { AnswerId = 35, Name = "Iron", IsCorrect = false, QuestionId = 9 },
            new Answer { AnswerId = 36, Name = "Quartz", IsCorrect = false, QuestionId = 9 },
            new Answer { AnswerId = 37, Name = "299,792,458 m/s", IsCorrect = true, QuestionId = 10 },
            new Answer { AnswerId = 38, Name = "150,000,000 m/s", IsCorrect = false, QuestionId = 10 },
            new Answer { AnswerId = 39, Name = "1,080,000,000 km/h", IsCorrect = false, QuestionId = 10 },
            new Answer { AnswerId = 40, Name = "100,000 km/s", IsCorrect = false, QuestionId = 10 },
            // Answers for History Questions
            new Answer { AnswerId = 41, Name = "George Washington", IsCorrect = true, QuestionId = 11 },
            new Answer { AnswerId = 42, Name = "Thomas Jefferson", IsCorrect = false, QuestionId = 11 },
            new Answer { AnswerId = 43, Name = "Abraham Lincoln", IsCorrect = false, QuestionId = 11 },
            new Answer { AnswerId = 44, Name = "John Adams", IsCorrect = false, QuestionId = 11 },
            new Answer { AnswerId = 45, Name = "1912", IsCorrect = true, QuestionId = 12 },
            new Answer { AnswerId = 46, Name = "1905", IsCorrect = false, QuestionId = 12 },
            new Answer { AnswerId = 47, Name = "1898", IsCorrect = false, QuestionId = 12 },
            new Answer { AnswerId = 48, Name = "1920", IsCorrect = false, QuestionId = 12 },
            new Answer { AnswerId = 49, Name = "Christopher Columbus", IsCorrect = true, QuestionId = 13 },
            new Answer { AnswerId = 50, Name = "Ferdinand Magellan", IsCorrect = false, QuestionId = 13 },
            new Answer { AnswerId = 51, Name = "Marco Polo", IsCorrect = false, QuestionId = 13 },
            new Answer { AnswerId = 52, Name = "James Cook", IsCorrect = false, QuestionId = 13 },
            new Answer { AnswerId = 53, Name = "Neil Armstrong", IsCorrect = true, QuestionId = 14 },
            new Answer { AnswerId = 54, Name = "Buzz Aldrin", IsCorrect = false, QuestionId = 14 },
            new Answer { AnswerId = 55, Name = "Yuri Gagarin", IsCorrect = false, QuestionId = 14 },
            new Answer { AnswerId = 56, Name = "Michael Collins", IsCorrect = false, QuestionId = 14 },
            new Answer { AnswerId = 57, Name = "American Civil War", IsCorrect = true, QuestionId = 15 },
            new Answer { AnswerId = 58, Name = "World War I", IsCorrect = false, QuestionId = 15 },
            new Answer { AnswerId = 59, Name = "World War II", IsCorrect = false, QuestionId = 15 },
            new Answer { AnswerId = 60, Name = "Spanish-American War", IsCorrect = false, QuestionId = 15 },
            // Answers for Geography Questions
            new Answer { AnswerId = 61, Name = "Nile River", IsCorrect = true, QuestionId = 16 },
            new Answer { AnswerId = 62, Name = "Amazon River", IsCorrect = false, QuestionId = 16 },
            new Answer { AnswerId = 63, Name = "Yangtze River", IsCorrect = false, QuestionId = 16 },
            new Answer { AnswerId = 64, Name = "Mississippi River", IsCorrect = false, QuestionId = 16 },
            new Answer { AnswerId = 65, Name = "Asia", IsCorrect = true, QuestionId = 17 },
            new Answer { AnswerId = 66, Name = "Africa", IsCorrect = false, QuestionId = 17 },
            new Answer { AnswerId = 67, Name = "North America", IsCorrect = false, QuestionId = 17 },
            new Answer { AnswerId = 68, Name = "Europe", IsCorrect = false, QuestionId = 17 },
            new Answer { AnswerId = 69, Name = "China", IsCorrect = true, QuestionId = 18 },
            new Answer { AnswerId = 70, Name = "India", IsCorrect = false, QuestionId = 18 },
            new Answer { AnswerId = 71, Name = "USA", IsCorrect = false, QuestionId = 18 },
            new Answer { AnswerId = 72, Name = "Indonesia", IsCorrect = false, QuestionId = 18 },
            new Answer { AnswerId = 73, Name = "Australia", IsCorrect = true, QuestionId = 19 },
            new Answer { AnswerId = 74, Name = "Europe", IsCorrect = false, QuestionId = 19 },
            new Answer { AnswerId = 75, Name = "South America", IsCorrect = false, QuestionId = 19 },
            new Answer { AnswerId = 76, Name = "Antarctica", IsCorrect = false, QuestionId = 19 },
            new Answer { AnswerId = 77, Name = "Sweden", IsCorrect = true, QuestionId = 20 },
            new Answer { AnswerId = 78, Name = "Finland", IsCorrect = false, QuestionId = 20 },
            new Answer { AnswerId = 79, Name = "Norway", IsCorrect = false, QuestionId = 20 },
            new Answer { AnswerId = 80, Name = "Canada", IsCorrect = false, QuestionId = 20 },
            // Answers for Math Questions
            new Answer { AnswerId = 81, Name = "3.14", IsCorrect = true, QuestionId = 21 },
            new Answer { AnswerId = 82, Name = "2.71", IsCorrect = false, QuestionId = 21 },
            new Answer { AnswerId = 83, Name = "1.62", IsCorrect = false, QuestionId = 21 },
            new Answer { AnswerId = 84, Name = "0", IsCorrect = false, QuestionId = 21 },
            new Answer { AnswerId = 85, Name = "4", IsCorrect = true, QuestionId = 22 },
            new Answer { AnswerId = 86, Name = "3", IsCorrect = false, QuestionId = 22 },
            new Answer { AnswerId = 87, Name = "5", IsCorrect = false, QuestionId = 22 },
            new Answer { AnswerId = 88, Name = "6", IsCorrect = false, QuestionId = 22 },
            new Answer { AnswerId = 89, Name = "4", IsCorrect = true, QuestionId = 23 },
            new Answer { AnswerId = 90, Name = "5", IsCorrect = false, QuestionId = 23 },
            new Answer { AnswerId = 91, Name = "6", IsCorrect = false, QuestionId = 23 },
            new Answer { AnswerId = 92, Name = "7", IsCorrect = false, QuestionId = 23 },
            new Answer { AnswerId = 93, Name = "9.8 m/s^2", IsCorrect = true, QuestionId = 24 },
            new Answer { AnswerId = 94, Name = "3.14 m/s^2", IsCorrect = false, QuestionId = 24 },
            new Answer { AnswerId = 95, Name = "2.71 m/s^2", IsCorrect = false, QuestionId = 24 },
            new Answer { AnswerId = 96, Name = "1.62 m/s^2", IsCorrect = false, QuestionId = 24 },
            new Answer { AnswerId = 97, Name = "2x", IsCorrect = true, QuestionId = 25 },
            new Answer { AnswerId = 98, Name = "x^2", IsCorrect = false, QuestionId = 25 },
            new Answer { AnswerId = 99, Name = "1/x", IsCorrect = false, QuestionId = 25 },
            new Answer { AnswerId = 100, Name = "x", IsCorrect = false, QuestionId = 25 }
        };
        return answers;
    }
}