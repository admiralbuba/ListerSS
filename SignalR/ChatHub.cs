using ListerSS.Database;
using ListerSS.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ListerSS.SignalR
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub<IChatHub>
    {
        private readonly ListerContext _db;
        public ChatHub(ListerContext db)
        {
            _db = db;
        }
        public async Task Send(MessageDto message)
        {
            await Clients.Users(message.ToName).Receive(message);
        }

        public async Task Enter(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).Notify($"{Context.User.FindFirstValue(ClaimTypes.Name)} joined the group {groupName}");
        }

        public async Task SendGroup(MessageDto message)
        {
            await Clients.OthersInGroup(message.ToName).ReceiveGroup(message);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.Notify($"Greetings {Context.User.FindFirstValue(ClaimTypes.Name)}");
            await base.OnConnectedAsync();
        }
    }
}