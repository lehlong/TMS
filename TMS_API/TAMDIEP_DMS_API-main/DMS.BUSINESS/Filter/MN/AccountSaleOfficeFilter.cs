using Common;

namespace DMS.BUSINESS.Filter.MN
{
    public class AccountSaleOfficeFilter: BaseFilter
    {
        public string? UserName { get; set; }

        public int[]? PartnerIds { get; set; }

        public int? ProvineId { get; set; }

        public int? DistrictId { get; set; }

        public int? WardId { get; set; }

        public string? SaleType { get; set; }

        public int? SupplierId { get; set; }
    }
}
