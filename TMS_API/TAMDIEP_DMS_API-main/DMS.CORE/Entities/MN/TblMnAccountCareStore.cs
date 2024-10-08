using DMS.CORE.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DMS.CORE.Entities.MD;

namespace DMS.CORE.Entities.MN
{
    [Table("T_MN_ACCOUNT_CARE_STORE")]
    public class TblMnAccountCareStore : BaseEntity
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("PLAN_ID")]
        public int PlanId { get; set; }

        [Column("PARTNER_ID")]
        public int PartnerId { get; set; }

        [Column("CONTENTS", TypeName = "CLOB")]
        public string Contents { get; set; }

        [Column("ACTION_DATE")]
        public DateTime ActionDate { get; set; }

        [Column("REFRENCE_ID", TypeName = "VARCHAR2(40)")]
        public Guid RefrenceId { get; set; }

        [ForeignKey("PartnerId")]
        public virtual TblMdPartner Partner { get; set; }

        [ForeignKey("PlanId")]
        public virtual TblMnAccountPlanVisit AccountPlanVisit { get; set; }
    }
}
