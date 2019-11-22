using BusinessLogic.Services.Interfaces;
using CommonLogic.Logger;
using DataProvider;
using DataProvider.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Services.HostedServices
{
    public class NotificationCleaner : IHostedService
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;

        public NotificationCleaner(INotificationService notificationService, ILoggerFactory loggerFactory)
        {
            _notificationService = notificationService;
            _logger = loggerFactory.CreateLogger<FileLogger>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            do
            {
                try
                {
                    await _notificationService.RemoveDeliveredNotificationsAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError("NotificationCleaner, could not process cleanup", e);
                }

                await Task.Delay(1000 * 60 * 5, cancellationToken);
            }
            while (!cancellationToken.IsCancellationRequested);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
