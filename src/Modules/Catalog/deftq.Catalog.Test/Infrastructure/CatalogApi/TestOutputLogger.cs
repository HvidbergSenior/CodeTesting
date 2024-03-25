using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace deftq.Catalog.Test.Infrastructure.CatalogApi
{
    public class TestOutputLogger<T> : ILogger<T> 
    {
        private readonly ITestOutputHelper _output;

        public TestOutputLogger(ITestOutputHelper output)
        {
            _output = output;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new Disposable();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            _output.WriteLine($"{DateTime.Now} {logLevel} {formatter.Invoke(state, exception)}");
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        private class Disposable : IDisposable
        {
            public void Dispose()
            {
                    
            }
        }
    }
}
