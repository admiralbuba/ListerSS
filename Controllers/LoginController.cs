using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpPost]
        [Route("auth")]
        public async Task<ActionResult<Token>> Auth(string name)
        {
            if (!names.Contains(name))
                return BadRequest("User does not exist");

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("SecretKey_SecretKey_SecretKey"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Name", name)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = "MyServer",
                Audience = "MyClient",
                SigningCredentials = credentials
            };

            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
            var handler = new JwtSecurityTokenHandler();

            return new Token(handler.WriteToken(token));
        }
    }
    public record Token(string Bearer);
}