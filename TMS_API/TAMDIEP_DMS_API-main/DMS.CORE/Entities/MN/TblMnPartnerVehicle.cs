using DMS.CORE.Common;
using DMS.CORE.Entities.MD;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MN
{
    [Table("T_MN_PARTNER_VEHICLE")]
    public class TblMnPartnerVehicle : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("VEHICLE_CODE")]
        public string VehicleCode { get; set; }

        [Column("PARTNER_ID")]
        public int PartnerId { get; set; }

        public virtual TblMdVehicle Vehicle { get; set; }

        public virtual TblMdPartner Partner { get; set; }
    }
}
