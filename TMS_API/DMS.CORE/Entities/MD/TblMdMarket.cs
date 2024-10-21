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
    [Table("T_MD_MARKET")]
    public class TblMdMarket : SoftDeleteEntity
    {
        [Key]
        [Column("CODE", TypeName = "VARCHAR(50)")]
        public string Code { get; set; }

        [Column("NAME", TypeName = "NVARCHAR(255)")]
        public string Name { get; set; }

        [Column("GAP", TypeName = "DECIMAL(18,0)")]
        public decimal? Gap { get; set; }

        [Column("LOCAL_CODE", TypeName = "VARCHAR(50)")]
        public string LocalCode { get; set; }

    }
}
