using Common;

namespace DMS.BUSINESS.Filter.MN
{
    public class ContractFilter : BaseFilter
    {

    }

    public class ContractGetAllFilter : BaseMdFilter
    {
        public int? PartnerId { get; set; }
    }
}
