using Common;

namespace XHTD.BUSINESS.Filter.XHTD
{
    public class StoreOrderOperatingFilter : BaseFilter
    {
        public List<int>? Steps { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public DateTime? WeightInTime { get; set; }

        public string? DriverUserName { get; set; }

        public bool? IsVoiced { get; set; }

        public List<string>? DeliveryCodes { get; set;}

        public string? Rfid { get; set; }

        public string? TypeProduct { get; set; }
    }
}
