using ListerSS.Models;

namespace ListerSS.SignalR
{
    public interface IChatHub
    {
        Task EnterGroup(HubMessage message);
        Task OnConnectedAsync();
        Task ReceiveMessage(HubMessage message);
        Task SendToGroup(HubMessage message);
    }
}