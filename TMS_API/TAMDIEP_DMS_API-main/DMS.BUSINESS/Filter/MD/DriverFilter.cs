using Common;

namespace DMS.BUSINESS.Filter.MD
{
    public class DriverFilter : BaseFilter
    {
        public int? PartnerIdCreate { get; set; }

        public string? VehicleCode { get; set; }
    }

    public class DriverGetAllFilter : BaseMdFilter
    {
        public int? PartnerIdCreate { get; set; }

        public string? VehicleCode { get; set; }
    }
}
