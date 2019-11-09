using CommonLogic.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System.IO;

namespace CommonLogic.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseFileLogger(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            string applicationDirectory = Directory.GetCurrentDirectory();
            string logsDirectory = "logs";
            loggerFactory.AddFile(Path.Combine(applicationDirectory, logsDirectory));
        }
    }
}
