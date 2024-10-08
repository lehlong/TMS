using DMS.CORE.Common;
using DMS.CORE.Entities.MN;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_PARTNER")]
    public class TblMdPartner : ReferenceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("CODE", TypeName = "VARCHAR2(50)")]
        public string Code { get; set; }

        [Column("NAME", TypeName = "NVARCHAR2(255)")]
        public string Name { get; set; }

        [Column("SALE_TYPE_CODE")]
        public string SaleTypeCode { get; set; }

        [Column("IS_CUSTOMER")]
        public bool IsCustomer { get; set; }

        [Column("IS_SUPPLIER")]
        public bool IsSupplier { get; set; }

        [Column("ADDRESS", TypeName = "NVARCHAR2(255)")]
        public string? Address { get; set; }

        [Column("PHONE_NUMBER", TypeName = "VARCHAR2(15)")]
        public string? PhoneNumber { get; set; }

        [Column("EMAIL", TypeName = "VARCHAR2(155)")]
        public string? Email { get; set; }

        [Column("PROVINCE_ID")]
        public int? ProvineId { get; set; }

        [Column("DISTRICT_ID")]
        public int? DistrictId { get; set; }

        [Column("WARD_ID")]
        public int? WardId { get; set; }

        [Column("DESCRIPTION")]
        public string? Description { get; set; }

        [Column("LONGITUDE", TypeName = "VARCHAR2(50)")]
        public string? Longitude { get; set; }

        [Column("LATITUDE", TypeName = "VARCHAR2(50)")]
        public string? Latitude { get; set; }

        [Column("REPRESENTATIVE", TypeName = "VARCHAR2(155)")]
        public string? Representative { get; set; }

        [Column("TAX_NUMBER", TypeName = "VARCHAR2(20)")]
        public string? TaxNumber { get; set; }

        public virtual TblMdProvine Provine { get; set; }

        public virtual TblMdDistrict District { get; set; }

        public virtual TblMdWard Ward { get; set; }

        public virtual TblMdSaleType SaleType { get; set; }

        public virtual List<TblMnPartnerReference> ReferenceBuys { get; set; }

        public virtual List<TblMnPartnerReference> ReferenceSells { get; set; }

        public virtual List<TblMnPartnerVehicle> VehicleReferences { get; set; }

        public virtual List<TblMnPartnerArea> AreaReferences { get; set; }

        public virtual List<TblMnAccountSaleOffice> AccountSaleOffices { get; set; }
    }
}
