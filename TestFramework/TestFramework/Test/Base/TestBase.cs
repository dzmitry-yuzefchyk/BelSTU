using OpenQA.Selenium.Support.Extensions;
using Serilog;
using System;
using System.IO;
using TestFramework.Driver;

namespace TestFramework.Test.Base
{
    public class TestBase
    {
        public void Test(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                var screenshot = DriverController.Driver().TakeScreenshot();
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "artifacts\\");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                screenshot.SaveAsFile($"{filePath}artifact_{screenshot.GetHashCode()}.png");

                var logsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs\\", "log.txt");
                var logger = new LoggerConfiguration()
                    .WriteTo
                    .File(logsPath, fileSizeLimitBytes: 1024 * 1024, rollOnFileSizeLimit: true)
                    .CreateLogger();
                logger.Error(e.Message);
            }
        }
    }
}
