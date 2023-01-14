using Lister.Persistence.Database;
using Lister.WebApi.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;
using System.Security.Claims;

namespace Lister.WebApi.SignalR
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub<IChatHub>
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ListerContext _db;
        public ChatHub(ListerContext db, IConnectionMultiplexer redis)
        {
            _db = db;
            _redis = redis;
        }
        public async Task Send(MessageResponse message)
        {
            await Clients.Users(message.ToName).Receive(message);
        }

        public async Task Enter(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).Notify($"{Context.User.FindFirstValue(ClaimTypes.Name)} joined the group {groupName}");
        }

        public async Task SendGroup(MessageResponse message)
        {
            await Clients.OthersInGroup(message.ToName).ReceiveGroup(message);
        }

        public override async Task OnConnectedAsync()
        {
            var name = Context.User.FindFirstValue(ClaimTypes.Name);
            var user = await _db.Users.FindAsync();
            await _redis.GetDatabase().HashSetAsync(user.Id.ToString(), new HashEntry[]
            {
                new HashEntry("ConnectionId", Context.ConnectionId),
                new HashEntry("ConnectedAt", DateTime.UtcNow.ToString())
                });
            await Clients.All.Notify($"Greetings {name}");
            await base.OnConnectedAsync();
        }
    }
}