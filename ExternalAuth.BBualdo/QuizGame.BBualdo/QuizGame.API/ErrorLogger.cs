using QuizGame.Data.Models;
using QuizGame.Data.Services;

namespace QuizGame.API;

public class ErrorLogger(IErrorsService errorsService) : ILogger
{
    private readonly IErrorsService _errorsService = errorsService;

    public IDisposable? BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (logLevel == LogLevel.None) return;

        var error = new Error
        {
            Message = $"{logLevel.ToString()}: {formatter(state, exception)}",
            Time = DateTime.Now.ToUniversalTime()
        };

        _errorsService.AddError(error);
    }
}