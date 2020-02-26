using Microsoft.Extensions.Logging;

namespace CommonLogic.Logger
{
    public static class LoggerFactoryExtension
    {
        public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory, string filePath)
        {
            loggerFactory.AddProvider(new FileLoggerProvider(filePath));
            return loggerFactory;
        }
    }
}
