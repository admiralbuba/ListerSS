﻿using AutoMapper;
using Lister.Domain.Models;
using Lister.Persistence.Database;
using Lister.WebApi.Models.Request;
using Lister.WebApi.Models.Response;
using Lister.WebApi.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;
using System.Security.Claims;

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
        public async Task<ActionResult<CreateGroupResponse>> Create(CreateGroupRequest request)
        {
            //var userId = HttpContext.User.FindFirstValue("Id");
            //request.Users.Add(Guid.Parse(userId));

            var users = _db.Users.Where(x => request.Users.Contains(x.Guid)).ToList();

            var group = new Group() { Name = request.Name, Users = users };

            _db.Groups.Add(group);
            await _db.SaveChangesAsync();

            var response = _mapper.Map<CreateGroupResponse>(group);
            return response;
        }
    }
}
