using DataProvider;
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
        private readonly TaskboardContext _context;

        public static List<SignalrUser> Users = new List<SignalrUser>();

        public NotificationHub(TaskboardContext taskboardContext)
        {
            _context = taskboardContext;
        }

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;

            if (Users.SingleOrDefault(x => x.ConnectionId == connectionId) == null)
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

        public void MarkAsRead(int notificationId)
        {
            try
            {
                var notification = _context.Notifications.Find(notificationId);
                _context.Remove(notification);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
            }
        }
    }

    public class SignalrUser
    {
        public string ConnectionId { get; set; }
        public Guid IdentityUserId { get; set; }
    }
}
