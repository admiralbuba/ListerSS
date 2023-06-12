using AutoMapper;
using Lister.Domain.Models;
using Lister.Persistence.Database;
using Lister.WebApi.Models.Request;
using Lister.WebApi.Models.Response;
using Lister.WebApi.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Lister.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GroupsController : ControllerBase
    {
        private readonly IHubContext<ChatHub, IChatHub> _chatHubContext;
        private readonly ILogger<LoginController> _logger;
        private readonly IConnectionMultiplexer _redis;
        private readonly ListerContext _db;
        private readonly IMapper _mapper;

        public GroupsController(ILogger<LoginController> logger, IHubContext<ChatHub,
            IChatHub> chatHubContext,
            IConnectionMultiplexer redis,
            ListerContext db,
            IMapper mapper)
        {
            _logger = logger;
            _chatHubContext = chatHubContext;
            _redis = redis;
            _db = db;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Models.Response.CreateGroup>> CreateGroup(Models.Request.CreateGroup request)
        {
            var users = await _db.Users.Where(x => request.Users.Contains(x.Guid)).ToListAsync();

            var group = new Domain.Models.Group() { Name = request.Name, Users = users };

            _db.Groups.Add(group);
            await _db.SaveChangesAsync();

            foreach (var user in users)
            {
                var connId = await _redis.GetDatabase().HashGetAsync(user.Guid.ToString(), "ConnectionId");
                if (connId.HasValue)
                {
                    await _chatHubContext.Groups.AddToGroupAsync(connId, group.Name);
                    await _chatHubContext.Clients.Groups(group.Name).Notify($"{user.Name} joined the group {group.Name}");
                }
            }

            var response = _mapper.Map<Models.Response.CreateGroup>(group);
            return response;
        }

        [HttpPatch]
        [Route("addUsers")]
        public async Task<ActionResult<Models.Response.AddUsers>> AddUsersToGroup(Models.Request.AddUsers request)
        {
            var group = await _db.Groups.Include(x => x.Users).SingleOrDefaultAsync(x => x.Guid == request.Id);
            
            if(group == null)
                return BadRequest("Group does not exist");

            var users = _db.Users.Where(x => request.Users.Contains(x.Guid));
            
            var newUsers = users.Except(group.Users).ToList();

            group.Users.AddRange(users);
            await _db.SaveChangesAsync();

            foreach (var user in newUsers)
            {
                var connId = await _redis.GetDatabase().HashGetAsync(user.Guid.ToString(), "ConnectionId");
                if (connId.HasValue)
                {
                    await _chatHubContext.Groups.AddToGroupAsync(connId, group.Name);
                    await _chatHubContext.Clients.Groups(group.Name).Notify($"{user.Name} joined the group {group.Name}");
                }
            }

            var response = new Models.Response.AddUsers
            {
                Users = newUsers.Select(x => x.Guid).ToList(),
                ModifiedAt = DateTime.Now
            };

            return response;
        }
    }
}
