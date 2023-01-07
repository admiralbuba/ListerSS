using ListerSS.Models;
using ListerSS.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ListerSS.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private List<string> names = new List<string>() { "qwe", "ewq" };

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("auth")]
        public async Task<ActionResult<Token>> Auth(string name)
        {
            if (!names.Contains(name))
                return BadRequest("User does not exist");

            return new Token(JwtUtils.CreateToken(name));
        }
    }
}