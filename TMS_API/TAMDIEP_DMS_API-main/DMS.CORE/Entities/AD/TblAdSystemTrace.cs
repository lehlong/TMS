using DMS.CORE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_SYSTEM_TRACE")]
    public class TblAdSystemTrace : BaseEntity
    {
        [Key]
        [Column("CODE", TypeName = "varchar2(50)")]
        public string Code { get; set; }

        [Column("NAME", TypeName = "nvarchar2(255)")]
        public string Name { get; set; }

        [Column("TYPE", TypeName = "varchar2(50)")]
        public string Type { get; set; }

        [Column("ADDRESS", TypeName = "varchar2(255)")]
        public string Address { get; set; }

        [Column("INTERVAL")]
        public int InterVal { get; set; }

        [Column("NOTE", TypeName = "nvarchar2(500)")]
        public string? Note { get; set; }

        [Column("STATUS")]
        public bool? Status { get; set; }

        [Column("LOG", TypeName = "nvarchar2(500)")]
        public string? Log { get; set; }

        [Column("LAST_CHECK_TIME")]
        public DateTime? LastCheckTime { get; set;}
    }
}
