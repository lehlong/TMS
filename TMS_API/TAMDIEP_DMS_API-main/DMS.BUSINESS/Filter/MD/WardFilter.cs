using Common;

namespace DMS.BUSINESS.Filter.MD
{
    public class WardFilter : BaseFilter
    {
        public int? DistrictId { get; set; }
    }

    public class WardGetAllFilter : BaseMdFilter
    {
        public int? DistrictId { get; set; }
    }
}
