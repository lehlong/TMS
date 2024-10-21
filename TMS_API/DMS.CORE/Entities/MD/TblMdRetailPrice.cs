﻿using DMS.CORE.Common;
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

        [Column("GOODS_CODE", TypeName = "VARCHAR(50)")]    
        public string GoodsCode { set; get; }

        [Column("LOCAL_CODE", TypeName = "VARCHAR(50)")]
        public string LocalCode { set; get; }

        [Column("TO_DATE", TypeName = "DATETIME")]
        public DateTime ToDate { set; get; }

        [Column("FROM_DATE", TypeName = "DATETIME")]
        public DateTime? FromDate { set; get; }

        [Column("OLD_PRICE", TypeName = "FLOAT")]
        public float OldPrice { set; get; }

        [Column("NEW_PRICE", TypeName = "FLOAT")]
        public float NewPrice { set; get; }

    }
}
