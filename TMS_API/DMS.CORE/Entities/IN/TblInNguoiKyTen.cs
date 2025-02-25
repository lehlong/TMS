using DMS.CORE.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.IN
{
    [Table("T_IN_NGUOI_KY_TEN")]
    public class TblInNguoiKyTen : BaseEntity
    {
        [Key]
        [Column("CODE", TypeName = "VARCHAR(50)")]
        public string Code { get; set; }

        [Column("HEADER_CODE", TypeName = "VARCHAR(50)")]
        public string HeaderCode { set; get; }

        [Column("DEPARTMENT", TypeName = "VARCHAR(250)")]
        public string Department { get; set; }

        [Column("POSITION", TypeName = "VARCHAR(250)")]
        public string Position { get; set; }

        [Column("SIGNATORY", TypeName = "VARCHAR(250)")]
        public string Signatory { get; set; }
    }
}
