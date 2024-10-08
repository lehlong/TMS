using DMS.CORE.Common;
using DMS.CORE.Entities.MD;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MN
{
    [Table("T_MN_PARTNER_CONTRACT")]
    public class TblMnPartnerContract : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("PARTNER_ID")]
        public int PartnerId { get; set; }

        [Column("CONTRACT_ID", TypeName = "VARCHAR2(50)")]
        public string ContractId { get; set; }

        [ForeignKey("ContractId")]
        public virtual TblMnContract Contract { get; set; } 

        [ForeignKey("PartnerId")]
        public virtual TblMdPartner Partner { get; set; }
    }
}
