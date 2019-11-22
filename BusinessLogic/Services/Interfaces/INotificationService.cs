using BusinessLogic.Models.Notification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationViewModel>> GetNotificationsAsync(Guid userId);
        Task<bool> CreateNotificationAsync(CreateNotificationModel model);
        Task<bool> MarkAsDeliveredAsync(int notificationId);
        Task<int> RemoveDeliveredNotificationsAsync();
    }
}
