using DMS.CORE.Common;
using DMS.CORE.Entities.MD;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MN
{
    [Table("T_MN_PARTNER_REFERENCE")]
    public class TblMnPartnerReference : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("PARTNER_ID_BUY")]
        public int PartnerIdBuy { get; set; }

        [Column("PARTNER_ID_SELL")]
        public int PartnerIdSell { get; set; }

        public virtual TblMdPartner PartnerBuy { get; set; }

        public virtual TblMdPartner PartnerSell { get; set; }

    }
}
