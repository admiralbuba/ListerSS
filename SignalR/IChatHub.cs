using ListerSS.Dto;
using ListerSS.Models;

namespace ListerSS.SignalR
{
    public interface IChatHub
    {
        Task Receive(MessageDto message);
        Task Notify(string groupName);
        Task ReceiveGroup(MessageDto message);
    }
}