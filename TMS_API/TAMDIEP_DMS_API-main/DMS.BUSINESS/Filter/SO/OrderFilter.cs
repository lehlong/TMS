using Common;
namespace DMS.BUSINESS.Filter.SO
{
    public class OrderFilter : BaseFilter
    {
        public int? PartnerIdBuy { get; set; }

        public int? PartnerIdSell { get; set; }

        public string? VehicleCode { get; set; }

        public int? AreaId { get; set; }

        public string? ItemCode { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }    

        public List<string>? States { get; set; }

        public string? Type { get; set; }

        public string? ParentCode { get; set; }

        public bool? HasParent { get; set; }
    }

    public class OrderGetAllFilter : BaseMdFilter
    {
        public int? PartnerIdBuy { get; set; }

        public int? PartnerIdSell { get; set; }

        public string? Type { get; set; }

        public string? ItemCode { get; set; }

        public int? AreaId { get; set; }

        public List<string>? States { get; set; }

        public string? ParentCode { get; set; }

        public bool? HasParent { get; set; }
    }
}
