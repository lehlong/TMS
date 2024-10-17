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
    [Table("T_MD_LAI_GOP_DIEU_TIET")]
    public class TblMdLaiGopDieuTiet : SoftDeleteEntity
    {
        [Key]
        [Column("CODE", TypeName = "VARCHAR(50)")]
        public string Code { set; get; }

        [Column("GOODS_CODE", TypeName = "VARCHAR(50)")]
        public string GoodsCode { set; get; }

        [Column("MARKET_CODE", TypeName = "VARCHAR(50)")]
        public string MarketCode { set; get; }

        [Column("TO_DATE", TypeName = "DATETIME")]
        public DateTime ToDate { set; get; }

        [Column("CREATE_DATE", TypeName = "DATETIME")]
        public DateTime? CreateDate { set; get; }

        [Column("PRICE", TypeName = "FLOAT")]
        public float Price { set; get; }

    }
}
