using Lister.Persistence.Database;
using Lister.WebApi.Models.Response;
using Lister.WebApi.Utils;
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

        public LoginController(ILogger<LoginController> logger, ListerContext db)
        {
            _logger = logger;
            _db = db;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authentication")]
        public async Task<ActionResult<TokenResponse>> Authenticate(string name)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Name == name);
            if (user == null)
                return BadRequest("User does not exist");

            return new TokenResponse(JwtUtils.CreateToken(user));
        }
    }
}