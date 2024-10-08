using Common;

namespace DMS.BUSINESS.Filter.MN
{
    public class AccountPlanVisitFilter : BaseFilter
    {
        public int? Year { get; set; }

        public int? Month { get; set; }

        public string? AccountSaleOffice { get; set; }
    }

    public class AccountPlanVisitUserFilter
    {
        public int? Year { get; set; }

        public int? Month { get; set; }

        public string UserName { get; set; }

        public string? KeyWord { get; set; }
    }
}
