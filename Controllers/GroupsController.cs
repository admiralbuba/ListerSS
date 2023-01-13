using AutoMapper;
using ListerSS.Database;
using ListerSS.Models;
using ListerSS.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace ListerSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(Group group)
        {
            var grp = _db.Groups.Add(group);
            await _db.SaveChangesAsync();
            return Ok(grp.Entity);
        }
    }
}
