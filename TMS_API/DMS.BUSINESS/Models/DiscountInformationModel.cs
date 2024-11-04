using DMS.CORE.Entities.MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Models
{
    public class DiscountInformationModel
    {
        public List<TblMdGoods> lstGoods { get; set; }
        public List<discout> discount { get; set; } = new List<discout>();
    }

    public class discout
    {
        public string colA { get; set; }
        public string colB { get; set; }
        public decimal? col1 { get; set; }
        public decimal? col2 { get; set; }
        public decimal? col3 { get; set; }
        public decimal? col4 { get; set; }
        public decimal? col5 { get; set; }
        public decimal? col6 { get; set; }
        public List<CK> CK { get; set; } = new List<CK>();

    }
    public class CK
    {
        public decimal? plxna { get; set; }
        public List<CkCl> AP { get; set; } = new List<CkCl>();
        public List<CkCl> VA { get; set; } = new List<CkCl>();
    }

    public class CkCl
    {
        public decimal? ck { get; set; }
        public decimal? cl { get; set; }
    }
}
