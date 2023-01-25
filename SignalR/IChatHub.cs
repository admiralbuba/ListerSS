using Lister.WebApi.Models.Response;

namespace Lister.WebApi.SignalR
{
    public interface IChatHub
    {
        Task Receive(Message message);
        Task Notify(string groupName);
        Task ReceiveGroup(Message message);
    }
}