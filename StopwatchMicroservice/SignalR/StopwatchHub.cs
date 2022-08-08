using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace StopwatchMicroservice
{
    public class StopwatchHub: Hub
    {
        public async Task SendTime(int auctionId, int time)
        {
            await Clients.All.SendAsync("ReceiveTime", auctionId, time);
        }

    }
}
