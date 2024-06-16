using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizGame.Data.Models;
using QuizGame.Data.Services;
using QuizGame.Data.Services.DTO.QuizDTOs;

namespace QuizGame.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuizzesController(IQuizzesService quizzesService, ILogger logger) : ControllerBase
{
    private readonly IQuizzesService _quizzesService = quizzesService;
    private readonly ILogger _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizResponse>>> GetQuizzes()
    {
        IEnumerable<QuizResponse> quizzes = await _quizzesService.GetQuizzesAsync();
        return Ok(quizzes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuizDetailsResponse>> GetQuizById(int id)
    {
        QuizDetailsResponse? quiz = await _quizzesService.GetQuizByIdAsync(id);
        if (quiz == null)
        {
            _logger.LogError($"Tried to get quiz (ID = {id}) which doesn't exist.");
            return NotFound("Quiz not found.");
        }
        return Ok(quiz);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> AddQuiz(QuizRequest quizRequest)
    {
        await _quizzesService.AddQuizAsync(quizRequest);
        return CreatedAtAction(nameof(AddQuiz), quizRequest);
    }


    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<ActionResult> DeleteQuiz(int id)
    {
        await _quizzesService.DeleteQuizAsync(id);
        return NoContent();
    }
}