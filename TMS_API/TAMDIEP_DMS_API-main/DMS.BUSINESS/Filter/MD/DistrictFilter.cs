using Common;
namespace DMS.BUSINESS.Filter.MD
{
    public class DistrictFilter : BaseFilter
    {
        public int? ProvineId { get; set; }
    }

    public class DistrictGetAllFilter : BaseMdFilter
    {
        public int? ProvineId { get; set; }
    }
}
