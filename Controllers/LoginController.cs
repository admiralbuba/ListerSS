using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        [HttpPost]
        [Route("auth")]
        public async Task<ActionResult<Token>> Auth(string name)
        {
            if (!names.Contains(name))
                return BadRequest("User does not exist");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKey_SecretKey_SecretKey"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, name)
            };
            var token = new JwtSecurityToken("MyServer",
                "MyClient",
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            var res = new JwtSecurityTokenHandler().WriteToken(token);

            return new Token(res);
        }
    }
    public record Token(string Bearer);
}