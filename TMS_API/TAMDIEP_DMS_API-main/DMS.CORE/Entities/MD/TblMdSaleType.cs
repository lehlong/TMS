using DMS.CORE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_SALE_TYPE")]
    public class TblMdSaleType : BaseEntity
    {
        [Key]
        [Column("CODE",TypeName = "VARCHAR2(50)")]
        public string Code { get; set; }

        [Column("NAME",TypeName = "NVARCHAR2(155)")]
        public string Name { get; set; }

        public virtual List<TblMdPartner> Partners { get; set; }    
    }
}
