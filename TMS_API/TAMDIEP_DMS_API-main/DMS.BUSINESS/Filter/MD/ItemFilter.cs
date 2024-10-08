using Common;

namespace DMS.BUSINESS.Filter.MD
{
    public class ItemFilter : BaseFilter
    {
        public string? GroupCode { get; set;}

        public string? TypeCode { get; set; }

        public int? AreaId { get; set; }

        public int? PartnerId { get; set; }
    }

    public class ItemGetAllFilter : BaseMdFilter
    {
        public string? GroupCode { get; set; }

        public string? TypeCode { get; set; }

        public int? AreaId { get; set; }

        public int? PartnerId { get; set; }
    }
}
