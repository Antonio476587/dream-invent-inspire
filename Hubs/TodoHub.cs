using Microsoft.AspNetCore.SignalR;

namespace TodoApp.Hubs
{
    public class TodoHub : Hub
    {
        public async Task SendSignal() {
            await Clients.Others.SendAsync("ReloadPage");
        }
    }
}