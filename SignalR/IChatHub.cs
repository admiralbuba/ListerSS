using Lister.WebApi.Models.Response;

namespace Lister.WebApi.SignalR
{
    public interface IChatHub
    {
        Task Receive(MessageResponse message);
        Task Notify(string groupName);
        Task ReceiveGroup(MessageResponse message);
    }
}