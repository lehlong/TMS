using DMS.CORE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_GROUP_ITEM")]
    public class TblMdGroupItem : BaseEntity
    {
        [Key]
        [Column("CODE",TypeName = "VARCHAR2(50)")]
        public string Code { get; set; }

        [Column("NAME", TypeName = "NVARCHAR2(255)")]
        public string Name { get; set; }

        [Column("ORDER_NUMBER")]
        public int OrderNumber { get; set; }
    }
}
