using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.SO
{
    [Table("T_SO_ORDER_PROCESS")]
    public class TblSoOrderProcess
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public Guid Id { get; set; }

        [Column("ORDER_CODE")]
        public string OrderCode { get; set; }

        [Column("PROCESS_DATE")]
        public DateTime ProcessDate { get; set; }

        [Column("ACTION_CODE",TypeName = "VARCHAR2(20)")]
        public string? ActionCode { get; set; }

        [Column("PREV_STATE", TypeName = "VARCHAR2(20)")]
        public string? PrevState { get; set; }

        [Column("STATE", TypeName = "VARCHAR2(20)")]
        public string? State { get; set; }

        [ForeignKey("OrderCode")]
        public virtual TblSoOrder Order { get; set; }
    }
}
