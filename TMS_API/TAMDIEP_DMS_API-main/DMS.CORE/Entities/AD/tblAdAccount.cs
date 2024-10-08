using DMS.CORE.Common;
using DMS.CORE.Entities.MD;
using DMS.CORE.Entities.MN;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_ACCOUNT")]
    public class TblAdAccount : SoftDeleteEntity
    {
        [Key]
        [Column("USER_NAME",TypeName = "VARCHAR2(50)")]
        public string UserName { get; set; }

        [Column("FULL_NAME",TypeName = "NVARCHAR2(255)")]
        public string FullName { get; set; }

        [Column("PASSWORD", TypeName = "VARCHAR2(50)")]
        public string Password { get; set; }

        [Column("PHONE_NUMBER",TypeName = "VARCHAR2(10)")]
        public string? PhoneNumber { get; set; }

        [Column("EMAIL",TypeName = "VARCHAR2(255)")]
        public string? Email { get; set; }

        [Column("ADDRESS",TypeName = "NVARCHAR2(255)")]
        public string? Address { get; set; }

        [Column("ACCOUNT_TYPE", TypeName = "VARCHAR2(10)")]
        public string? AccountType { get; set; }

        [Column("PARTNER_ID")]
        public int? PartnerId { get; set; }

        [Column("DRIVER_ID")]
        public int? DriverId { get; set; }

        [ForeignKey("PartnerId")]
        public virtual TblMdPartner Partner { get; set; }

        [ForeignKey("DriverId")]
        public virtual TblMdDriver Driver { get; set; }

        public virtual ICollection<TblAdAccount_AccountGroup> Account_AccountGroups { get; set; }

        public virtual ICollection<TblAdAccountRight> AccountRights { get; set; }

        public virtual ICollection<TblAdAccountRefreshToken> RefreshTokens { get; set; }

        public virtual ICollection<TblMnAccountSaleOffice> AccountSaleOffices { get; set; }

    }
}
