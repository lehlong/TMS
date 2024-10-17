using DMS.CORE.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_RETAIL_PRICE")]
    public class TblMdRetailPrice : SoftDeleteEntity
    {
        [Key]
        [Column("CODE", TypeName = "VARCHAR(50)")]
        public string Code { set; get; }

        [Column("TYPE_OF_GOODS_CODE", TypeName = "VARCHAR(50)")]
        public string TypeOfGoodsCode { set; get; }

        [Column("LOCAL_CODE", TypeName = "VARCHAR(50)")]
        public string LocalCode { set; get; }

        [Column("TO_DATE", TypeName = "DATETIME")]
        public DateTime ToDate { set; get; }

        [Column("CREATE_DATE", TypeName = "DATETIME")]
        public DateTime? CreateDate { set; get; }

        [Column("OLD_PRICE", TypeName = "VARCHAR(50)")]
        public string OldPrice { set; get; }

        [Column("NEW_PRICE", TypeName = "VARCHAR(50)")]
        public string NewPrice { set; get; }

    }
}
