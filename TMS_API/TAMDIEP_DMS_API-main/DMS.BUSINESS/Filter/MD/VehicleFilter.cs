using Common;

namespace DMS.BUSINESS.Filter.MD
{
    public class VehicleFilter : BaseFilter
    {
        public string? VehicleTypeCode { get; set; }

        public int? PartnerIdCreate { get; set; }

        public int? PartnerId { get; set; }
    }

    public class VehicleGetAllFilter : BaseMdFilter
    {
        public string? VehicleTypeCode { get; set; }

        public int? PartnerId { get; set; }
    }
}
