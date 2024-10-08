using DMS.CORE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MN
{
    [Table("T_MN_CONTRACT")]
    public class TblMnContract : BaseEntity
    {
        [Key]
        [Column("CONTRACT_ID", TypeName = "VARCHAR2(50)")]
        public string ContractId { get; set; }

        public List<TblMnPartnerContract> PartnerContracts { get; set; }
    }
}
