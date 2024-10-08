using Common;
using Microsoft.AspNetCore.SignalR;
using XHTD.BUSINESS.Dtos.Hub;

namespace XHTD.BUSINESS.Hub
{
    public interface IGatewayHub : IBaseService
    {
        Task SendGatewayNotification(SendGatewayNotificationDto model);
    }

    public class GatewayHub(IHubContext<GatewayHub> hubContext) : Microsoft.AspNetCore.SignalR.Hub, IGatewayHub
    {
        private readonly IHubContext<GatewayHub> _hubContext = hubContext;

        public MessageObject MessageObject { get; set; } = new();
        public Exception Exception { get; set; }
        public bool Status { get; set; } = true;

        public async Task SendGatewayNotification(SendGatewayNotificationDto model)
        {
            await _hubContext.Clients.All.SendAsync(SignalRMethod.Gateway.ToString(), model);
        }
    }
}
