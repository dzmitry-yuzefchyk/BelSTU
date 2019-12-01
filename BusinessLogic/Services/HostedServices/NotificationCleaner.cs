using BusinessLogic.Services.Interfaces;
using CommonLogic.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Services.HostedServices
{
    public class NotificationCleaner : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public NotificationCleaner(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<FileLogger>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //Yep
            return Task.Factory.StartNew(() => ExecuteAsync(cancellationToken));
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("NotificationCleaner started");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Yield();
                    _logger.LogInformation("NotificationCleaner cleanup");
                    using var scope = _serviceProvider.CreateScope();
                    var notificationService = scope.ServiceProvider.GetService<INotificationService>();
                    await notificationService.RemoveDeliveredNotificationsAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError("NotificationCleaner, could not process cleanup", e);
                }

                await Task.Delay(1000 * 60 * 5, cancellationToken);
            }

            _logger.LogInformation("NotificationCleaner stopped");
        }
    }
}
