using DMS.CORE.Common;
using DMS.CORE.Entities.MD;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MN
{
    [Table("T_MN_ACCOUNT_PLAN_VISIT_STORE")]
    public class TblMnAccountPlanVisitStore : BaseEntity
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("PLAN_ID")]
        public int PlanId { get; set; }

        [Column("PARTNER_ID")]
        public int PartnerId { get; set; }

        [ForeignKey("PlanId")]
        public virtual TblMnAccountPlanVisit AccountPlanVisit { get; set; }

        [ForeignKey("PartnerId")]
        public virtual TblMdPartner Partner { get; set; }
    }
}
