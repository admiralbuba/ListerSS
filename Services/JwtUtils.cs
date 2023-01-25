using Lister.Domain.Models;
using Lister.WebApi.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lister.WebApi.Services
{
    public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration _config;
        public JwtUtils(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(User user)
        {
            var auth = _config.GetSection("Authentication").Get<Authentication>();
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(auth.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new Claim("Id", user.Guid.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };
            var jwt = new JwtSecurityToken(
                    issuer: auth.ValidIssuer,
                    audience: auth.ValidAudience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
