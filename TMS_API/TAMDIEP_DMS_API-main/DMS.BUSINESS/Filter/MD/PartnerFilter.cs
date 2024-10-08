using Common;
namespace DMS.BUSINESS.Filter.MD
{
    public class PartnerFilter : BaseFilter
    {
        public int? ProvineId { get; set; }

        public int? DistrictId { get; set; }

        public int? WardId { get; set; }

        public bool? IsCustomer { get; set; }

        public bool? IsSupplier { get; set; }

        public string? SaleType { get; set; }

        public int? SupplierId { get; set; }

        public int? CustomerId { get; set; }

        public int? ReferenceId { get; set; }

        public List<int>? ExcludePartner { get; set; }
    }

    public class PartnerGetAllFilter : BaseMdFilter
    {
        public bool? IsCustomer { get; set; }

        public bool? IsSupplier { get; set; }

        public string? SaleType { get; set; }

        public int? ReferenceId { get; set; }

        public bool? IsSaleTypeC2C3 { get; set; }
    }
}
