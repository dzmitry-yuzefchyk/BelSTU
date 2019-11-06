using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace CommonLogic.Logger
{
    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        private static readonly object _lock = new object();

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null) return;

            lock (_lock)
            {
                Directory.CreateDirectory(_filePath);
                string message = $"{formatter(state, exception)}\n";
                string fileName = $"{DateTime.Now.ToString("dd-MM-yyyy")}.log.txt";
                File.AppendAllText(Path.Combine(_filePath, fileName), $"{logLevel.ToString()}: {message}");
            }
        }
    }
}
