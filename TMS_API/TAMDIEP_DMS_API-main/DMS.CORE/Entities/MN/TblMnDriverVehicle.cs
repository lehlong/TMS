using DMS.CORE.Common;
using DMS.CORE.Entities.MD;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MN
{
    [Table("T_MN_DRIVER_VEHICLE")]
    public class TblMnDriverVehicle : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("VEHICLE_CODE")]
        public string VehicleCode { get; set; }

        [Column("PARTNER_ID")]
        public int DriverId { get; set; }

        [ForeignKey("VehicleCode")]
        public virtual TblMdVehicle Vehicle { get; set; }

        [ForeignKey("DriverId")]
        public virtual TblMdDriver Driver { get; set; }
    }
}
