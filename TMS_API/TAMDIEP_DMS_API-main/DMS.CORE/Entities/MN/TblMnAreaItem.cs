using DMS.CORE.Common;
using DMS.CORE.Entities.MD;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MN
{
    [Table("T_MN_AREA_ITEM")]
    public class TblMnAreaItem : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ITEM_CODE")]
        public string ItemCode { get; set; }

        [Column("AREA_ID")]
        public int AreaId { get; set; }

        [ForeignKey("AreaId")]
        public virtual TblMdArea Area { get; set; }

        [ForeignKey("ItemCode")]
        public virtual TblMdItem Item { get; set; }
    }
}
