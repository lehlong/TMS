using DMS.CORE.Common;
using DMS.CORE.Entities.MN;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_VEHICLE")]
    public class TblMdVehicle : ReferenceEntity
    {
        [Key]
        [Column("CODE",TypeName = "VARCHAR2(50)")]
        public string Code { get; set; }

        [Column("VEHICLE_TYPE_CODE")]
        public string VehicleTypeCode { get; set; }

        [Column("TONNAGE")]
        public double? Tonnage { get; set; }

        [Column("TARE_TONNAGE")]
        public double? TareTonnage { get; set; }

        [Column("HEIGHT")]
        public double? Height { get; set; }

        [Column("WIDTH")]
        public double? Width { get; set; }

        [Column("LENGTH")]
        public double? Length { get; set; }

        [Column("PARTNER_ID_CREATE")]
        public int? PartnerIdCreate { get; set; }

        public virtual TblMdVehicleType Type { get; set; }

        public virtual TblMdPartner PartnerCreate { get; set; }

        public List<TblMnPartnerVehicle> PartnerReferences { get; set; }

        public List<TblMnDriverVehicle> DriverReferences { get; set; }

    }
}
