using BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessLogic.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly INotificationService _notificationService;

        public static List<SignalrUser> Users { get; set; }

        public NotificationHub(INotificationService notificationService)
        {
            Users = new List<SignalrUser>();
            _notificationService = notificationService;
        }

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;

            if (Users.Any(x => x.ConnectionId == connectionId))
            {
                var userId = Guid.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                Users.Add(new SignalrUser { ConnectionId = connectionId, IdentityUserId = userId });
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var id = Context.ConnectionId;
            var userId = Guid.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            SignalrUser user = Users.Find(x => x.IdentityUserId == userId);
            Users.Remove(user);

            return base.OnDisconnectedAsync(exception);
        }

        public async void MarkAsRead(int notificationId)
        {
            await _notificationService.MarkAsDeliveredAsync(notificationId);
        }
    }

    public class SignalrUser
    {
        public string ConnectionId { get; set; }
        public Guid IdentityUserId { get; set; }
    }
}
