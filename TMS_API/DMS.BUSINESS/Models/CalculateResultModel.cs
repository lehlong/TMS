using DMS.CORE.Entities.MD;
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
        public DLG DLG { get; set; } = new DLG();
        public List<DB> DB { get; set; } = new List<DB>();
        public List<PT09> PT09 { get; set; } = new List<PT09>();
        public List<PL4> PL4 { get; set; } = new List<PL4>();
    }
    public class DLG
    {
        public List<DLG_1> Dlg_1 { get; set; } = new List<DLG_1>();
        public List<DLG_2> Dlg_2 { get; set; } = new List<DLG_2>();
        public List<DLG_3> Dlg_3 { get; set; } = new List<DLG_3>();
        public List<DLG_4> Dlg_4 { get; set; } = new List<DLG_4>();
        public List<DLG_5> Dlg_5 { get; set; } = new List<DLG_5>();
        public List<DLG_6> Dlg_6 { get; set; } = new List<DLG_6>();

    }
    public class DLG_1
    {
        public string Code { get; set; }
        public string Col1 { get; set; }
        public decimal? Col2 { get; set; }
        public decimal? Col3 { get; set; }
        public decimal? Col4 { get; set; }
        public decimal? Col5 { get; set; }
        public decimal? Col6 { get; set; }
        public decimal? Col7 { get; set; }
    }
    public class DLG_2
    {
        public string Code { get; set; }
        public string Col1 { get; set; }
        public decimal? Col2 { get; set; }
    }
    public class DLG_3
    {
        public string Code { get; set; }
        public string ColA { get; set; }
        public string ColB { get; set; }
        public decimal? Col1 { get; set; }
        public decimal? Col2 { get; set; }
        public decimal? Col3 { get; set; }
        public decimal? Col4 { get; set; }
        public decimal? Col5 { get; set; }
        public decimal? Col6 { get; set; }
        public decimal? Col7 { get; set; }
        public decimal? Col8 { get; set; }
        public decimal? Col9 { get; set; }
        public decimal? Col10 { get; set; }
    }
    public class DLG_4
    {
        public string Code { get; set; }
        public string Type { get; set; }   
        public string ColA { get; set; }
        public string ColB { get; set; }
        public decimal? Col1 { get; set; }
        public decimal? Col2 { get; set; }
        public decimal? Col3 { get; set; }
        public decimal? Col4 { get; set; }
        public decimal? Col5 { get; set; }
        public decimal? Col6 { get; set; }
        public decimal? Col7 { get; set; }
        public decimal? Col8 { get; set; }
        public decimal? Col9 { get; set; }
        public decimal? Col10 { get; set; }
        public decimal? Col11 { get; set; }
        public decimal? Col12 { get; set; }
        public decimal? Col13 { get; set; }
        public decimal? Col14 { get; set; }
        public decimal? Col15 { get; set; }
        public decimal? Col16 { get; set; }
        public bool IsBold { get; set; } = false;
    }
    public class DLG_5
    {
        public string Code { get; set; }
        public string ColA { get; set; }
        public string ColB { get; set; }
        public decimal? Col1 { get; set; }
        public decimal? Col2 { get; set; }
        public decimal? Col3 { get; set; }
        public decimal? Col4 { get; set; }
        public decimal? Col5 { get; set; }
    }
    public class DLG_6
    {
        public string Code { get; set; }
        public string ColA { get; set; }
        public string ColB { get; set; }
        public decimal? Col1 { get; set; }
        public decimal? Col2 { get; set; }
        public decimal? Col3 { get; set; }
        public decimal? Col4 { get; set; }
        public decimal? Col5 { get; set; }
        public decimal? Col6 { get; set; }
        public decimal? Col7 { get; set; }
        public string Col8 { get; set; }

    }
    public class PT
    {
        public string Code { get; set; }
        public string ColA { get; set; }
        public string ColB { get; set; }
        public decimal? Col1 { get; set; }
        public List<decimal?> LG { get; set; } = new List<decimal?>();
        public List<decimal?> LN { get; set; } = new List<decimal?>();
        public List<PT_GG> GG { get; set; } = new List<PT_GG>();
        public List<PT_BVMT> BVMT { get; set; } = new List<PT_BVMT>();
        public decimal? Col3 { get; set; }
        public decimal? Col4 { get; set; }
        public decimal? Col5 { get; set; }
        public decimal? Col6 { get; set; }
        public int Order { get; set; }
        public bool IsBold { get; set; }

    }
    public class PT_GG
    {
        public decimal? VAT { get; set; }
        public decimal? NonVAT { get; set; }
    }
    public class PT_BVMT
    {
        public decimal? VAT { get; set; }
        public decimal? NonVAT { get; set; }
    }
    public class DB
    {
        public string Code { get; set; }
        public string ColA { get; set; }
        public string ColB { get; set; }
        public string Col1 { get; set; }
        public decimal? Col2 { get; set; }
        public List<decimal?> LG { get; set; } = new List<decimal?>();
        public decimal? Col3 { get; set; }
        public decimal? Col4 { get; set; }
        public decimal? Col5 { get; set; }
        public decimal? Col6 { get; set; }
        public decimal? Col7 { get; set; }
        public decimal? Col8 { get; set; }
        public decimal? Col9 { get; set; }
        public decimal? Col10 { get; set; }
        public List<PT_GG> GG { get; set; } = new List<PT_GG>();

        public bool IsBold { get; set; } = false;
    }
    public class FOB
    {

    }
    public class PT09
    {
        public string Code { get; set; }
        public string ColA { get; set; }
        public string ColB { get; set; }
        public List<decimal?> LG { get; set; } = new List<decimal?>();
        public decimal? Col3 { get; set; }
        public decimal? Col4 { get; set; }
        public decimal? Col5 { get; set; }
        public decimal? Col6 { get; set; }
        public decimal? Col7 { get; set; }
        public decimal? Col8 { get; set; }
        public List<PT09_GG> GG { get; set; } = new List<PT09_GG>();
        public decimal? Col18 { get; set; }
        public List<decimal?> LN { get; set; } = new List<decimal?>();
    }
    public class PT09_GG
    {
        public decimal? VAT { get; set; }
        public decimal? NonVAT { get; set; }
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
        public string Code { get; set; }
        public string ColA { get; set; }
        public string ColB { get; set; }
        public List<decimal?> GG { get; set; } = new List<decimal?>();
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
        public string? FDate { get; set; }
    }
}
