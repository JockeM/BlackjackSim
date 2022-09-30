// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging;

public class ConsoleLogger : ILogger
{
    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        throw new NotImplementedException();
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        Console.WriteLine(formatter(state, exception));
    }
}
