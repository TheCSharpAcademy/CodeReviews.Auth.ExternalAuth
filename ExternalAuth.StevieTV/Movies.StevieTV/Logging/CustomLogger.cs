using Movies.StevieTV.Data;
using Movies.StevieTV.Models;

namespace Movies.StevieTV.Logging;

public class CustomLogger : ILogger
{
    private readonly MoviesContext _moviesContext;

    public CustomLogger(MoviesContext moviesContext)
    {
        _moviesContext = moviesContext;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (logLevel == LogLevel.None)
            return;

        var errorLog = new ErrorLog()
        {
            Error = $"{logLevel.ToString()}: {formatter(state, exception)}",
            Time = DateTime.Now
        };

        _moviesContext.ErrorLog.Add(errorLog);
        _moviesContext.SaveChanges();
    }
}