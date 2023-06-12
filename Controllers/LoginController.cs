using Lister.Persistence.Database;
using Lister.WebApi.Models.Response;
using Lister.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lister.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ListerContext _db;
        private readonly IJwtUtils _jwt;

        public LoginController(ILogger<LoginController> logger, ListerContext db, IJwtUtils jwt)
        {
            _logger = logger;
            _db = db;
            _jwt= jwt;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authentication")]
        public async Task<ActionResult<Token>> Authenticate(string name)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Name == name);
            if (user == null)
                return BadRequest("User does not exist");

            return new Token(_jwt.CreateToken(user));
        }
    }
}