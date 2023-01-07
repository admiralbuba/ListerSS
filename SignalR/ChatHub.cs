using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Security.Claims;

namespace ListerSS.SignalR
{
    public class ChatHub : Hub
    {
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task Send(string username, string message)
        {
            var user = Context.User;
            var userClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await Clients.All.SendAsync("Receive", username, message);
        }
    }
}