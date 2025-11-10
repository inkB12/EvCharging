using Microsoft.AspNetCore.SignalR;

namespace EVCharging.Hubs
{
    public class StationHub : Hub
    {
        public async Task SendStationCreated(object e)
        {
            await Clients.All.SendAsync("StationCreated", e);
        }

        public async Task SendStationUpdated(object e)
        {
            await Clients.All.SendAsync("StationUpdated", e);
        }

        public async Task SendStationDeleted(object e)
        {
            await Clients.All.SendAsync("StationDeleted", e);
        }
    }
}
