using Common;
using Microsoft.AspNetCore.SignalR;
using XHTD.BUSINESS.Dtos.Hub;

namespace XHTD.BUSINESS.Hub
{
    public interface IScale1Hub : IBaseService
    {
        Task SendSensor(SendSensorScaleNotificationDto model);
        Task SendMessage(SendMessageScaleNotificationDto model);
        Task SendScale1Info(SendScaleInfo model);
    }

    public class Scale1Hub(IHubContext<Scale1Hub> hubContext) : Microsoft.AspNetCore.SignalR.Hub, IScale1Hub
    {
        private readonly IHubContext<Scale1Hub> _hubContext = hubContext;

        public MessageObject MessageObject { get; set; } = new();
        public Exception Exception { get; set; }
        public bool Status { get; set; } = true;

        public async Task SendSensor(SendSensorScaleNotificationDto model)
        {
            await _hubContext.Clients.All.SendAsync(SignalRMethod.SendSensor.ToString(), model);
        }

        public async Task SendMessage(SendMessageScaleNotificationDto model)
        {
            await _hubContext.Clients.All.SendAsync(SignalRMethod.SendMessage.ToString(), model);
        }

        public async Task SendScale1Info(SendScaleInfo model)
        {
            await _hubContext.Clients.All.SendAsync(SignalRMethod.SendScale1Info.ToString(), model);
        }
    }
}
