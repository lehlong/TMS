using DMS.CORE.Common;
using DMS.CORE.Entities.AD;
using DMS.CORE.Entities.MN;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_DRIVER")]
    public class TblMdDriver : ReferenceEntity
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("FULL_NAME", TypeName = "NVARCHAR2(255)")]
        public string FullName { get; set; }

        [Column("CCCD_NUMBER", TypeName = "VARCHAR2(20)")]
        public string? CccdNumber { get; set; }

        [Column("PHONE_NUMBER", TypeName = "VARCHAR2(20)")]
        public string? PhoneNumber { get; set; }

        [Column("NOTES", TypeName = "NVARCHAR2(510)")]
        public string? Notes { get; set; }

        [Column("USER_NAME")]
        public string? UserName { get; set; }

        [Column("PARTNER_ID_CREATE")]
        public int? PartnerIdCreate { get; set; }

        [ForeignKey("PartnerIdCreate")]
        public virtual TblMdPartner PartnerCreate { get; set; }

        [ForeignKey("UserName")]
        public virtual TblAdAccount Account { get; set; }

        public List<TblMnDriverVehicle> VehicleReferences { get; set; }
    }
}
