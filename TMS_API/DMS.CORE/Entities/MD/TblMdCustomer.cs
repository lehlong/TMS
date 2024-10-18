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
    [Table("T_MD_CUSTOMER")]
    public class TblMdCustomer : SoftDeleteEntity
    {
        [Key]
        [Column("CODE", TypeName = "VARCHAR(50)")]
        public string Code { set; get; }

        [Column("NAME", TypeName = "NVARCHAR(255)")]
        public string Name { set; get; }

        [Column("PHONE", TypeName = "FLOAT")]
        public float Phone { set; get; }

        [Column("EMAIL", TypeName = "NVARCHAR(255)")]
        public string Email { set; get; }

        [Column("ADDRESS", TypeName = "NVARCHAR(255)")]
        public string Address { set; get; }

        [Column("BUY_INFO", TypeName = "NVARCHAR(500)")]
        public string BuyInfo { set; get; }

        [Column("BANK_LOAN_INTEREST", TypeName = "FLOAT")]
        public float BankLoanInterest { set; get; }

        [Column("SALES_METHOD_CODE", TypeName = "VARCHAR(50)")]
        public string SaleMethodCode { set; get; }

        [Column("LOCAL_CODE", TypeName = "VARCHAR(50)")]
        public string LocalCode { set; get; }

        [Column("MARKER_CODE", TypeName = "VARCHAR(50)")]
        public string MarketCode { set; get; }


    }
}
