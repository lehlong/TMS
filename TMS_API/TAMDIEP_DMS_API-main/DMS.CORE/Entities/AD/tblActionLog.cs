using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMS.CORE.Common;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_ACTIONLOG")]
    public class TblActionLog : BaseEntity
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("USER_NAME",TypeName = "VARCHAR2(50)")]
        public string? UserName { get; set; }

        [Column("ACTION_URL",TypeName = "NVARCHAR2(255)")]
        public string ActionUrl { get; set; }

        [Column("REQUEST_DATA",TypeName = "CLOB")]
        public string RequestData { get; set; }

        [Column("REQUEST_TIME")]
        public DateTime? RequestTime { get; set; }

        [Column("RESPONSE_DATA",TypeName = "CLOB")]
        public string ResponseData { get; set; }

        [Column("RESPONSE_TIME")]
        public DateTime? ResponseTime { get; set; }

        [Column("STATUS_CODE")]
        public int StatusCode { get; set; }
    }
}