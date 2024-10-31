﻿using DMS.CORE.Common;
using DMS.CORE.Entities.BU;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.CORE.Entities.BU
{
    [Table("T_BU_DISCOINT_INFORMATION")]
    public class TblBuDiscountInformation : SoftDeleteEntity
    {
        [Key]
        [Column("CODE", TypeName = "VARCHAR(50)")]
        public string Code { get; set; }

        [Column("NAME", TypeName = "NVARCHAR(255)")]
        public string Name { get; set; }

        [Column("F_DATE", TypeName = "DATETIME")]
        public DateTime FDate { get; set; }

        [Column("STATUS", TypeName = "NVARCHAR(50)")]
        public string? Status { get; set; }
    }
}