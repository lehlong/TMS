using DMS.CORE.Common;
using DMS.CORE.Entities.MD;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MN
{
    [Table("T_MN_PARTNER_AREA")]
    public class TblMnPartnerArea : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("AREA_ID")]
        public int AreaId { get; set; }

        [Column("PARTNER_ID")]
        public int PartnerId { get; set; }

        [ForeignKey("AreaId")]
        public virtual TblMdArea Area { get; set; }

        [ForeignKey("PartnerId")]
        public virtual TblMdPartner Partner { get; set; }
    }
}
