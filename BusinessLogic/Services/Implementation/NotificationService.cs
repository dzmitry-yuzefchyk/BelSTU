using BusinessLogic.Models.Notification;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Logger;
using DataProvider;
using DataProvider.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly TaskboardContext _context;
        private readonly ILogger _logger;
        private readonly UserManager<User> _userManager;

        public NotificationService(TaskboardContext context,
            ILoggerFactory loggerFactory, UserManager<User> userManager)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<FileLogger>();
            _userManager = userManager;
        }

        public async Task<IEnumerable<NotificationViewModel>> GetNotificationsAsync(Guid userId)
        {
            try
            {
                var notDeliverednotifications = await _context.Notifications
                    .Where(x => x.RecipientId == userId && !x.IsDelivered)
                    .ToListAsync();
                var notifications = notDeliverednotifications
                            .Select(x => new NotificationViewModel
                            {
                                Description = x.Description,
                                DirectLink = x.DirectLink,
                                Id = x.Id,
                                Subject = x.Subject
                            }).ToList();

                return notifications;
            }
            catch (Exception e)
            {
                _logger.LogError("NotificationService, GetNotificationsAsync", e);
            }

            return null;
        }

        public async Task<bool> CreateNotificationAsync(CreateNotificationModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.RecipientEmail);
                var notification = new Notification
                {
                    RecipientId = user.Id,
                    Description = model.Description,
                    DirectLink = model.DirectLink,
                    Subject = model.Subject,
                    IsDelivered = false
                };

                _context.Notifications.Add(notification);
                var result = await _context.SaveChangesAsync();
                if (result < 0)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning("NotificationService, CreateNotificationAsync:", e);
                return false;
            }

            return true;
        }

        public async Task<bool> MarkAsDeliveredAsync(Guid notificationId)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(notificationId);
                notification.IsDelivered = true;
                _context.Attach(notification);
                _context.Entry(notification).Property(x => x.IsDelivered).IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogWarning("NotificationService, MarkAsDeliveredAsync:", e);
                return false;
            }

            return true;
        }
    }
}
