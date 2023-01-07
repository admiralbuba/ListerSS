﻿using ListerSS.Models;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ListerSS.SignalR
{
    public class ChatHub : Hub
    {
        //[Authorize]
        public async Task Send(HubMessage message)
        {
            var user = Context.User.FindFirstValue("Id");
            if (user is not null)
            {
                await Clients.Users(message.ToName, user).SendAsync("Receive", message.Text);
            }
        }

        public async Task Enter(HubMessage message)
        {
            var username = Context.User.FindFirstValue("Name");
            await Groups.AddToGroupAsync(Context.ConnectionId, message.ToName);
            await Clients.Group(message.ToName).SendAsync("Notify", $"{username} joined the group {message.ToName}");
        }

        public async Task SendGroup(HubMessage message)
        {
            await Clients.Group(message.ToName).SendAsync("ReceiveGroup", message.Text);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", $"Greetings {Context.User.FindFirstValue("Name")}");
            await base.OnConnectedAsync();
        }
    }
}