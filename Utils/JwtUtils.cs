using Lister.Domain.Models;
using Lister.WebApi.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lister.WebApi.Utils
{
    public class JwtUtils
    {
        public static string CreateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConfigHelper.Authentication.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new Claim("Id", user.Guid.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };
            var jwt = new JwtSecurityToken(
                    issuer: ConfigHelper.Authentication.ValidIssuer,
                    audience: ConfigHelper.Authentication.ValidAudience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
