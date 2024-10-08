using DMS.CORE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_PROVINE")]
    public class TblMdProvine : BaseEntity
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column("NAME",TypeName = "NVARCHAR2(255)")]
        public string Name { get; set; }

        [Column("ORDER_NUMBER")]
        public int OrderNumber { get; set; }

        public virtual List<TblMdDistrict> Districts { get; set; }
    }
}
