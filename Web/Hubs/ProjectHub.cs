using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs
{
    public class ProjectHub : Hub
    {
        public async void AddToGroupAsync(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async void RemoveFromGroupAsync(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
