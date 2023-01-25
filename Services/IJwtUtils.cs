using Lister.Domain.Models;

namespace Lister.WebApi.Services
{
    public interface IJwtUtils
    {
        string CreateToken(User user);
    }
}