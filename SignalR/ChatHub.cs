using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using System.Security.Claims;

namespace ListerSS.SignalR
{
    public class ChatHub : Hub
    {
        [Authorize]
        public async Task Send(string message, string to)
        {
            var user = Context.User.FindFirstValue("Name");
            if (user is not null)
            {
                await Clients.Users(to, user).SendAsync("Receive", message, user);
            }
        }
    }
}