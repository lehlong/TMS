﻿using DMS.CORE.Entities.MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Models
{
    public class CalculateResultModel
    {
        public List<PT> PT { get; set; } = new List<PT>();
        public List<TblMdGoods> lstGoods { get; set; }
    }
    public class PT
    {
        public string Code { get; set; }
        public string ColA { get; set; }
        public string ColB { get; set; }
        public decimal? Col1 { get; set; }
        public List<decimal?> LG { get; set; } = new List<decimal?>();
        public int Order { get; set; }
        public bool IsBold { get; set; }

    }
    public class DB
    {

    }
    public class FOB
    {

    }
    public class PT09
    {

    }
    public class BBDO
    {

    }
    public class BBFO
    {

    }
    public class PL1
    {

    }
    public class PL2
    {

    }
    public class PL3
    {

    }
    public class PL4
    {

    }
    public class VK11PT
    {

    }
    public class VK11DB
    {

    }
    public class VK11FOB
    {

    }
    public class VK11TNPP
    {

    }
    public class PTS
    {

    }
    public class VK11BB
    {

    }
    public class Summary
    {

    }
    public class QueryModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
