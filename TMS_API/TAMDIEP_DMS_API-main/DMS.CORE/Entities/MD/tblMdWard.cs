using DMS.CORE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_WARD")]
    public class TblMdWard : BaseEntity
    {
        [Key]
        [Column("ID", TypeName = "VARCHAR2(50)")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column("NAME", TypeName = "NVARCHAR2(255)")]
        public string Name { get; set; }

        [Column("ORDER_NUMBER")]
        public int OrderNumber { get; set; }

        [Column("DISTRICT_ID")]
        public int? DistrictId { get; set; }

        public virtual TblMdDistrict District { get; set; }
    }
}
