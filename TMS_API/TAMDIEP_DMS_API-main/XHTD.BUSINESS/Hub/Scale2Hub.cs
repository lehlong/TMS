using Common;
using Microsoft.AspNetCore.SignalR;
using XHTD.BUSINESS.Dtos.Hub;

namespace XHTD.BUSINESS.Hub
{
    public interface IScale2Hub : IBaseService
    {
        Task SendSensor(SendSensorScaleNotificationDto model);
        Task SendMessage(SendMessageScaleNotificationDto model);
        Task SendScale2Info(SendScaleInfo model);
    }

    public class Scale2Hub(IHubContext<Scale2Hub> hubContext) : Microsoft.AspNetCore.SignalR.Hub, IScale2Hub
    {
        private readonly IHubContext<Scale2Hub> _hubContext = hubContext;

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

        public async Task SendScale2Info(SendScaleInfo model)
        {
            await _hubContext.Clients.All.SendAsync(SignalRMethod.SendScale2Info.ToString(), model);
        }
    }
}
