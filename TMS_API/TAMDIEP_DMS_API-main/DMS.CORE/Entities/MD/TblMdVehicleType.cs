using DMS.CORE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_VEHICLE_TYPE")]
    public class TblMdVehicleType : BaseEntity
    {
        [Key]
        [Column("CODE",TypeName = "VARCHAR2(50)")]
        public string Code { get; set; }

        [Column("NAME", TypeName = "VARCHAR2(155)")]
        public string Name { get; set; }
    }
}
