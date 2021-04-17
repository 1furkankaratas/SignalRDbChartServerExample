using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRDbChartServerExample.Hubs
{
    public class SaleHub:Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("receiveMessage", "Merhaba");
        }
    }
}