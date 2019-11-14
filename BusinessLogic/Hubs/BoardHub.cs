using Microsoft.AspNetCore.SignalR;

namespace BusinessLogic.Hubs
{
    public class BoardHub : Hub
    {
        public async void AddToGroupAsync(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
