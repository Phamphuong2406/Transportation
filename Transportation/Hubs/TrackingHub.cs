using Microsoft.AspNetCore.SignalR;

namespace Transportation.Hubs
{
    public class TrackingHub : Hub
    {
        public async Task SendLocationUpdate(object location)
        {
            await Clients.All.SendAsync("ReceiveLocationUpdate", location);
        }
    }
}
