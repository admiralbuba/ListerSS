using ListerSS.Models;
using ListerSS.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace ListerSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IHubContext<ChatHub, IChatHub> _chatHubContext;
        private readonly ILogger<LoginController> _logger;

        public GroupsController(ILogger<LoginController> logger, IHubContext<ChatHub, IChatHub> chatHubContext)
        {
            _logger = logger;
            _chatHubContext = chatHubContext;
        }

    //    [AllowAnonymous]
    //    [HttpPost]
    //    [Route("create")]
    //    public async Task<IActionResult> Create(string name)
    //    {
    //        //return 
    //    }
    }
}
