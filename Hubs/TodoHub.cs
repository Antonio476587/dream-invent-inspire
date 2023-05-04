using Microsoft.AspNetCore.SignalR;

namespace TodoApp.Hubs
{
    public class TodoHub : Hub
    {
        public async Task SendData() {
            await Clients.Others.SendAsync("ReceiveData", true);
        }
    }
}