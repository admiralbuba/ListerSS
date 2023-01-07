using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ListerSS.SignalR
{
    public class ChatHub : Hub
    {
        //[Authorize]
        public async Task Send(string username, string message)
        {
            var user = Context.User;
            var userName = user?.Identity?.Name;
            await Clients.All.SendAsync("Receive", username, message);
        }
    }
}