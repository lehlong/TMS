using DMS.CORE.Common;
using DMS.CORE.Entities.AD;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MN
{
    [Table("T_MN_ACCOUNT_PLAN_VISIT")]
    public class TblMnAccountPlanVisit : BaseEntity
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("USER_NAME")]
        public string UserName { get; set; }

        [Column("PLAN_NAME")]
        public string PlanName { get; set; }

        [Column("YEAR")]
        public int Year { get; set; }

        [Column("MONTH")]
        public int Month { get; set; }

        [ForeignKey("UserName")]
        public virtual TblAdAccount Account { get; set; }

        public List<TblMnAccountPlanVisitStore> AccountPlanVisitStores { get; set; }

        public List<TblMnAccountCareStore> AccountCareStores { get; set; }
    }
}
