using DMS.CORE.Common;
using DMS.CORE.Entities.MN;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_AREA")]
    public class TblMdArea : BaseEntity
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column("NAME", TypeName = "NVARCHAR2(255)")]
        public string Name { get; set; }

        public List<TblMnAreaItem> ItemReferences { get; set; }

        public List<TblMnPartnerArea> PartnerReferences { get; set; }
    }
}
