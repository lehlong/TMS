using DMS.CORE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_UNIT")]
    public class TblMdUnit : BaseEntity
    {
        [Key]
        [Column("ID", TypeName = "VARCHAR2(50)")]
        public string Id { get; set; }

        [Column("NAME", TypeName = "NVARCHAR2(255)")]
        public string Name { get; set; }
    }
}
