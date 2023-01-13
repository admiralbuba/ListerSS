using ListerSS.Models.Response;

namespace ListerSS.SignalR
{
    public interface IChatHub
    {
        Task Receive(MessageResponse message);
        Task Notify(string groupName);
        Task ReceiveGroup(MessageResponse message);
    }
}