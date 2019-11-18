using CommonLogic.Logger;
using DataProvider;
using DataProvider.Entities;
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
        private readonly TaskboardContext _context;
        private readonly ILogger _logger;

        public NotificationCleaner(TaskboardContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<FileLogger>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            do
            {
                try
                {
                    IQueryable<Notification> notifications = _context.Notifications.Where(x => x.IsDelivered);
                    _context.Notifications.RemoveRange(notifications);
                    await _context.SaveChangesAsync();
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
