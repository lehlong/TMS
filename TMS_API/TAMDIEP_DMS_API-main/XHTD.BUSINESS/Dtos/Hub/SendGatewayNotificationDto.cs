namespace XHTD.BUSINESS.Dtos.Hub
{
    public class SendGatewayNotificationDto
    {
        public int Status { get; set; }

        public string InOut { get; set; }

        public string CardNo { get; set; }

        public string Message { get; set; }

        public string? DeliveryCode { get; set; }
    }

}
