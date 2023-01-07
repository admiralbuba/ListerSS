using Microsoft.AspNetCore.SignalR;

namespace ListerSS.SignalR
{
    public class UserIdProvider : IUserIdProvider
    {
        public virtual string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("Name")?.Value;
        }
    }
}
