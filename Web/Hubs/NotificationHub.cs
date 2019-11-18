using BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly INotificationService _notificationService;
        public static List<string> users;

        public NotificationHub(INotificationService notificationService)
        {
            users = new List<string>();
            _notificationService = notificationService;
        }

        public override Task OnConnectedAsync()
        {
            var id = Context.ConnectionId;

            if (!users.Contains(id))
            {
                users.Add(id);
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var id = Context.ConnectionId;
            users.Remove(id);

            return base.OnDisconnectedAsync(exception);
        }

        public async void MarkAsRead(Guid notificationId)
        {
            await _notificationService.MarkAsDeliveredAsync(notificationId);
        }
    }
}
