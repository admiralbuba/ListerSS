using Lister.Persistence.Database;
using Lister.WebApi.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Security.Claims;
using System.Xml.Linq;

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
            var guid = Context.User.FindFirstValue("Id");

            await _redis.GetDatabase().HashSetAsync(guid, new HashEntry[]
            {
                new HashEntry("ConnectionId", Context.ConnectionId),
                new HashEntry("ConnectedAt", DateTime.UtcNow.ToString())
                });

            await Clients.All.Notify($"Greetings {name}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var name = Context.User.FindFirstValue(ClaimTypes.Name);
            var guid = Context.User.FindFirstValue("Id");

            await _redis.GetDatabase().KeyDeleteAsync(guid);

            await Clients.All.Notify($"{name} disconnected");
            await base.OnDisconnectedAsync(ex);
        }
    }
}