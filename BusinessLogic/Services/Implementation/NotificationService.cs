using BusinessLogic.Hubs;
using BusinessLogic.Models.Notification;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Configuration;
using CommonLogic.Logger;
using DataProvider;
using DataProvider.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IEmailSender _emailSender;
        private readonly IOptions<AppSettings> _settings;

        public NotificationService(TaskboardContext context, IHubContext<NotificationHub> hubContext,
            ILoggerFactory loggerFactory, UserManager<User> userManager, IEmailSender emailSender, IOptions<AppSettings> settings)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<FileLogger>();
            _userManager = userManager;
            _notificationHub = hubContext;
            _emailSender = emailSender;
            _settings = settings;
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

                var notificationModel = new NotificationViewModel
                {
                    Description = notification.Description,
                    DirectLink = notification.DirectLink,
                    Id = notification.Id,
                    Subject = notification.Subject
                };

                return await Notify(user.Id, notificationModel);
            }
            catch (Exception e)
            {
                _logger.LogWarning("NotificationService, CreateNotificationAsync:", e);
            }

            return false;
        }

        public async Task<bool> MarkAsDeliveredAsync(int notificationId)
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

        public async Task<int> RemoveDeliveredNotificationsAsync()
        {
            IQueryable<Notification> notifications = _context.Notifications.Where(x => x.IsDelivered);
            _context.Notifications.RemoveRange(notifications);
            return await _context.SaveChangesAsync();
        }

        private async Task<bool> Notify(Guid userId, NotificationViewModel model)
        {
            var connectionId = NotificationHub.Users.Find(x => x.IdentityUserId == userId)?.ConnectionId;
            var user = await _context.Users.FindAsync(userId);
            var profile = await _context.UserProfiles.FindAsync(userId);
            var settings = await _context.UserSettings.FindAsync(userId);

            if (connectionId == null && settings.EmailNotifications && user.EmailConfirmed)
            {
                return await SendEmailNotificationAsync(user.Email, profile.Tag, model);
            }
            else
            {
                await _notificationHub.Clients.Client(connectionId).SendAsync("notify", model);
                return true;
            }
        }

        private async Task<bool> SendEmailNotificationAsync(string email, string tag, NotificationViewModel model)
        {
            try
            {
                string message = $"Hello, @{tag}," +
                                $"you recieved these message because {model.Description}" +
                                $"click to navigate {_settings.Value.Host.Url}/{model.DirectLink}";
                await _emailSender.SendEmailAsync(email, model.Subject, message);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogWarning("NotificationService, SendEmailNotificationAsync:", e);
            }

            return false;
        }
    }
}
