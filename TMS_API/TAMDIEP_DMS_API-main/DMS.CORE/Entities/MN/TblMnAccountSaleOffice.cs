using DMS.CORE.Entities.MD;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DMS.CORE.Common;
using DMS.CORE.Entities.AD;

namespace DMS.CORE.Entities.MN
{
    [Table("T_MN_ACCOUNT_SALE_OFFICE")]
    public class TblMnAccountSaleOffice : BaseEntity
    {
        [Key]
        [Column("USER_NAME")]
        public string UserName { get; set; }

        [Key]
        [Column("PARTNER_ID")]
        public int PartnerId { get; set; }

        public virtual TblAdAccount Account { get; set; }

        public virtual TblMdPartner Partner { get; set; }

    }
}
