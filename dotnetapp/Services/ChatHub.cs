using Microsoft.AspNetCore.SignalR;

namespace dotnetapp.Services  // namespace needed
{
    public class ChatHub : Hub //Inheriting from hub
    {
        // This method can be invoked by connected clients to send a message.
        public async Task SendMessage(string user, string message)
        {
            // Broadcast the message to all connected client
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
