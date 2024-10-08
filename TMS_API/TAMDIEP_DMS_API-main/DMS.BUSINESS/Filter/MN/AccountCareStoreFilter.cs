using Common;

namespace DMS.BUSINESS.Filter.MN
{
    public class AccountCareStoreFilter : BaseFilter
    {
        public int? PartnerId { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string? UserName { get; set; }
    }
}
