using DMS.CORE.Common;
using DMS.CORE.Entities.MN;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_ITEM")]
    public class TblMdItem : BaseEntity
    {
        [Key]
        [Column("CODE", TypeName = "VARCHAR2(50)")]
        public string Code { get; set; }

        [Column("NAME", TypeName = "NVARCHAR2(255)")]
        public string Name { get; set; }

        [Column("GROUP_ITEM_CODE")]
        public string? GroupCode { get; set; }

        [Column("TYPE_ITEM_CODE")]
        public string? TypeCode { get; set; }

        [ForeignKey("GroupCode")]
        public virtual TblMdGroupItem Group { get; set; }

        [ForeignKey("TypeCode")]
        public virtual TblMdTypeItem Type { get; set; }

        public List<TblMnAreaItem> AreaReferences { get; set; }
    }
}
