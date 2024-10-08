using DMS.CORE.Common;
using DMS.CORE.Entities.MD;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.SO
{
    [Table("T_SO_ORDER_DETAIL")]
    public class TblSoOrderDetail : ReferenceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public Guid Id { get; set; }

        [Column("ORDER_CODE")]
        public string OrderCode { get; set; }

        [Column("ITEM_CODE")]
        public string ItemCode { get; set; }

        [Column("ORDER_NUMBER")]
        public double OrderNumber { get; set; }

        [Column("UNIT_CODE")]
        public string UnitCode { get; set; }

        [ForeignKey("OrderCode")]
        public virtual TblSoOrder Order { get; set; }

        [ForeignKey("ItemCode")]
        public virtual TblMdItem Item { get; set; }

        [ForeignKey("UnitCode")]
        public virtual TblMdUnit Unit { get; set; }
    }
}
