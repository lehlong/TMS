using AutoMapper;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.BU;
using DMS.CORE.Entities.BU;
using DMS.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.CORE.Entities.MD;
using DMS.BUSINESS.Dtos.MD;
using Common;
using Microsoft.AspNetCore.Http;
using DMS.BUSINESS.Models;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SMO;
using DocumentFormat.OpenXml.Office2016.Excel;
using NPOI.SS.Formula.Functions;

namespace DMS.BUSINESS.Services.BU
{
    public interface ICalculateResultService : IGenericService<TblMdGoods, GoodsDto>
    {
        Task<CalculateResultModel> GetResult(string code);
        Task<InsertModel> GetDataInput(string code);
        Task<List<TblBuHistoryAction>> GetHistoryAction(string code);
        Task UpdateDataInput(InsertModel model);
        void ExportExcel(ref MemoryStream outFileStream, string path, string headerId);
    }
    public class CalculateResultService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdGoods, GoodsDto>(dbContext, mapper), ICalculateResultService
    {
        public async Task<CalculateResultModel> GetResult(string code)
        {
            try
            {
                var data = new CalculateResultModel();
                var lstGoods = await _dbContext.TblMdGoods.OrderBy(x => x.CreateDate).ToListAsync();
                data.lstGoods = lstGoods;
                var lstMarket = await _dbContext.TblMdMarket.OrderBy(x => x.Code).ToListAsync();
                var lstLGDT = await _dbContext.TblMdLaiGopDieuTiet.ToListAsync();
                var lstCustomer = await _dbContext.TblMdCustomer.ToListAsync();

                var dataVCL = await _dbContext.TblInVinhCuaLo.Where(x => x.HeaderCode == code).ToListAsync();
                var dataHSMH = await _dbContext.TblInHeSoMatHang.Where(x => x.HeaderCode == code).ToListAsync();
                var mappingBBDO = await _dbContext.TblMdMapPointCustomerGoods.ToListAsync();
                var dataPoint = await _dbContext.TblMdDeliveryPoint.ToListAsync();
                if (dataVCL.Count() == 0 || dataHSMH.Count() == 0)
                {
                    return data;
                }
                #region DLG
                var _oDlg = 1;
                foreach (var g in lstGoods)
                {
                    var vcl = dataVCL.Where(x => x.GoodsCode == g.Code).ToList();
                    var hsmh = dataHSMH.Where(x => x.GoodsCode == g.Code).ToList();
                    data.DLG.Dlg_1.Add(new DLG_1
                    {
                        Code = g.Code,
                        Col1 = g.Name,
                        Col2 = vcl.Sum(x => x.GblcsV1),
                        Col3 = vcl.Sum(x => x.GblV2),
                        Col4 = vcl.Sum(x => x.V2_V1),
                        Col5 = vcl.Sum(x => x.MtsV1),
                        Col6 = vcl.Sum(x => x.Gny),
                        Col7 = vcl.Sum(x => x.Clgblv),
                    });
                    data.DLG.Dlg_2.Add(new DLG_2
                    {
                        Code = g.Code,
                        Col1 = g.Name,
                        Col2 = vcl.Sum(x => x.GblV2),
                    });
                    var _dlg3 = new DLG_3
                    {
                        Code = g.Code,
                        ColA = _oDlg.ToString(),
                        ColB = g.Name,
                        Col1 = hsmh.Sum(x => x.HeSoVcf),
                        Col2 = hsmh.Sum(x => x.ThueBvmt),
                        Col3 = hsmh.Sum(x => x.L15ChuaVatBvmt),
                        Col4 = hsmh.Sum(x => x.HeSoVcf) * hsmh.Sum(x => x.L15ChuaVatBvmt),
                        Col5 = (hsmh.Sum(x => x.ThueBvmt) + hsmh.Sum(x => x.HeSoVcf) * hsmh.Sum(x => x.L15ChuaVatBvmt)) * 1.1M,
                        Col6 = vcl.Sum(x => x.GblcsV1),
                        Col7 = vcl.Sum(x => x.GblV2),
                    };
                    if (_dlg3.Col7 != 0)
                    {
                        _dlg3.Col8 = _dlg3.Col7 / 1.1M - _dlg3.Col2 ?? 0;
                    }
                    _dlg3.Col9 = _dlg3.Col7 - _dlg3.Col5;
                    _dlg3.Col10 = _dlg3.Col8 - _dlg3.Col4;
                    data.DLG.Dlg_3.Add(_dlg3);

                    var _dlg5 = new DLG_5
                    {
                        Code = g.Code,
                        ColA = _oDlg.ToString(),
                        ColB = g.Name,
                        Col1 = hsmh.Sum(x => x.HeSoVcf),
                        Col2 = hsmh.Sum(x => x.ThueBvmt),
                        Col3 = hsmh.Sum(x => x.L15ChuaVatBvmt),
                        Col4 = hsmh.Sum(x => x.HeSoVcf) * hsmh.Sum(x => x.L15ChuaVatBvmt),
                        Col5 = (hsmh.Sum(x => x.ThueBvmt) + hsmh.Sum(x => x.HeSoVcf) * hsmh.Sum(x => x.L15ChuaVatBvmt)) * 1.1M,
                    };
                    data.DLG.Dlg_5.Add(_dlg5);

                    var _dlg6 = new DLG_6
                    {
                        Code = g.Code,
                        ColA = _oDlg.ToString(),
                        ColB = g.Name,
                        Col1 = hsmh.Sum(x => x.HeSoVcf),
                        Col2 = hsmh.Sum(x => x.ThueBvmt),
                        Col4 = hsmh.Sum(x => x.L15ChuaVatBvmt),
                        Col5 = 0,

                    };
                    if (_dlg6.Col1 != 0 && _dlg6.Col2 != 0)
                    {
                        _dlg6.Col3 = _dlg6.Col2 / _dlg6.Col1;
                    }
                    _dlg6.Col6 = _dlg6.Col4 + _dlg6.Col3;
                    _dlg6.Col7 = _dlg6.Col6 * 1.1M;
                    data.DLG.Dlg_6.Add(_dlg6);

                    _oDlg++;
                }

                data.DLG.Dlg_4.Add(new DLG_4
                {
                    ColA = "I",
                    ColB = "Vùng thị trường trung tâm",
                    IsBold = true
                });
                var _oI = 1;
                foreach (var g in lstGoods)
                {
                    var hsmh = dataHSMH.Where(x => x.GoodsCode == g.Code).ToList();
                    var dlg1 = data.DLG.Dlg_1.Where(x => x.Code == g.Code).ToList();
                    var i = new DLG_4
                    {
                        Code = g.Code,
                        Type = "TT",
                        ColA = _oI.ToString(),
                        ColB = g.Name,
                        Col1 = hsmh.Sum(x => x.HeSoVcf),
                        Col2 = hsmh.Sum(x => x.ThueBvmt),
                        Col3 = hsmh.Sum(x => x.L15ChuaVatBvmtNbl),
                        Col4 = hsmh.Sum(x => x.HeSoVcf) * hsmh.Sum(x => x.L15ChuaVatBvmtNbl),
                        Col5 = (hsmh.Sum(x => x.ThueBvmt) + hsmh.Sum(x => x.HeSoVcf) * hsmh.Sum(x => x.L15ChuaVatBvmtNbl)) * 1.1M,
                        Col6 = dlg1.Sum(x => x.Col6),
                        Col14 = hsmh.Sum(x => x.GiamGiaFob),
                        Col10 = 0
                    };
                    if (i.Col6 != 0)
                    {
                        i.Col7 = i.Col6 / 1.1M - i.Col2;
                    }
                    i.Col8 = i.Col6 - i.Col5;
                    if (i.Col8 != 0)
                    {
                        i.Col9 = i.Col8 / 1.1M;
                    }
                    i.Col11 = i.Col1 * i.Col10 * 1.1M;
                    i.Col13 = i.Col11 + i.Col9;
                    i.Col12 = i.Col13 * 1.1M;
                    i.Col15 = (i.Col12 - i.Col14) * i.Col1;
                    i.Col16 = i.Col12 - i.Col14;

                    data.DLG.Dlg_4.Add(i);
                    _oI++;
                }

                data.DLG.Dlg_4.Add(new DLG_4
                {
                    ColA = "II",
                    ColB = "Các vùng thị trường còn lại",
                    IsBold = true
                });
                var _oII = 1;
                foreach (var g in lstGoods)
                {
                    var hsmh = dataHSMH.Where(x => x.GoodsCode == g.Code).ToList();
                    var dlg1 = data.DLG.Dlg_3.Where(x => x.Code == g.Code).ToList();
                    var i = new DLG_4
                    {
                        Code = g.Code,
                        Type = "OTHER",
                        ColA = _oII.ToString(),
                        ColB = g.Name,
                        Col1 = hsmh.Sum(x => x.HeSoVcf),
                        Col2 = hsmh.Sum(x => x.ThueBvmt),
                        Col3 = hsmh.Sum(x => x.L15ChuaVatBvmtNbl),
                        Col4 = hsmh.Sum(x => x.HeSoVcf) * hsmh.Sum(x => x.L15ChuaVatBvmtNbl),
                        Col5 = (hsmh.Sum(x => x.ThueBvmt) + hsmh.Sum(x => x.HeSoVcf) * hsmh.Sum(x => x.L15ChuaVatBvmtNbl)) * 1.1M,
                        Col6 = dlg1.Sum(x => x.Col7),
                        Col14 = hsmh.Sum(x => x.GiamGiaFob),
                        Col10 = 0
                    };
                    if (i.Col6 != 0)
                    {
                        i.Col7 = i.Col6 / 1.1M - i.Col2;
                    }
                    i.Col8 = i.Col6 - i.Col5;
                    if (i.Col8 != 0)
                    {
                        i.Col9 = i.Col8 / 1.1M;
                    }
                    i.Col11 = i.Col1 * i.Col10 * 1.1M;
                    i.Col13 = i.Col11 + i.Col9;
                    i.Col12 = i.Col13 * 1.1M;
                    i.Col15 = (i.Col12 - i.Col14) * i.Col1;
                    i.Col16 = i.Col12 - i.Col14;
                    data.DLG.Dlg_4.Add(i);
                    _oII++;
                }

                #endregion

                #region PT
                var orderPT = 1;
                foreach (var l in lstMarket.Select(x => x.LocalCode).Distinct().ToList())
                {
                    var local = _dbContext.tblMdLocal.Find(l);
                    data.PT.Add(new PT
                    {
                        ColB = local.Name,
                        IsBold = true,
                    });
                    foreach (var m in lstMarket.Where(x => x.LocalCode == l).ToList())
                    {
                        var i = new PT
                        {
                            Code = m.Code,
                            ColA = orderPT.ToString(),
                            ColB = m.Name,
                            Col1 = m.Gap,
                            Col3 = m.CPChungChuaCuocVC + m.CuocVCBQ,
                            Col4 = m.CPChungChuaCuocVC,
                            Col5 = m.CuocVCBQ,
                            Col6 = 0,
                        };
                        var _2 = i.Col3;

                        var _pl1 = new PL1
                        {
                            Code = m.Code,
                            ColA = orderPT.ToString(),
                            ColB = m.Name,
                        };
                        data.PL1.Add(_pl1);
                        foreach (var _l in lstGoods)
                        {
                            //var _c = lstLGDT.Where(x => x.MarketCode == m.Code && x.GoodsCode == _l.Code);
                            var _1 = m.LocalCode == "V1" ? data.DLG.Dlg_4.Where(x => x.Type == "TT" && x.Code == _l.Code).Sum(x => x.Col13) : data.DLG.Dlg_4.Where(x => x.Type == "OTHER" && x.Code == _l.Code).Sum(x => x.Col13);
                            //var _1 = _c == null || _c.Count() == 0 ? 0 : _c.Sum(x => x.Price);
                            i.LG.Add(Math.Round(_1 ?? 0));

                            var p = m.LocalCode == "V1" ? data.DLG.Dlg_4.Where(x => x.Code == _l.Code && x.Type == "TT").Sum(x => x.Col14) : data.DLG.Dlg_4.Where(x => x.Code == _l.Code && x.Type == "OTHER").Sum(x => x.Col14);
                            var d = new PT_GG
                            {
                                Code = _l.Code,
                                VAT = p - i.Col5 * 1.1M + i.Col6
                            };
                            d.VAT = Math.Round(d.VAT == null ? 0M : d.VAT / 10 ?? 0) * 10;
                            d.NonVAT = d.VAT == 0 ? 0 : d.VAT / 1.1M;
                            d.NonVAT = Math.Round(d.NonVAT ?? 0);
                            i.GG.Add(d);

                            _pl1.GG.Add(d.VAT);


                            var _3 = d.NonVAT;
                            i.LN.Add(_1 - _2 - _3);

                            var _h = m.LocalCode == "V1" ? data.DLG.Dlg_4.Where(x => x.Code == _l.Code && x.Type == "TT").Sum(x => x.Col6) : data.DLG.Dlg_4.Where(x => x.Code == _l.Code && x.Type == "OTHER").Sum(x => x.Col6);
                            var _d = m.LocalCode == "V1" ? data.DLG.Dlg_4.Where(x => x.Code == _l.Code && x.Type == "TT").Sum(x => x.Col2) : data.DLG.Dlg_4.Where(x => x.Code == _l.Code && x.Type == "OTHER").Sum(x => x.Col2);

                            var _b = new PT_BVMT
                            {
                                Code = _l.Code,
                                VAT = Math.Round(_h - d.VAT ?? 0),
                            };
                            var _nVAT = _h - d.VAT != 0 ? (_h - d.VAT) / 1.1M - _d : 0;
                            _b.NonVAT = Math.Round(_nVAT ?? 0);
                            i.BVMT.Add(_b);




                        }
                        data.PT.Add(i);
                        orderPT++;
                    }
                }
                #endregion

                #region ĐB

                var _oDb = 1;
                foreach (var v in lstCustomer.Where(x => x.CustomerTypeCode == "KBM").OrderBy(x => x.LocalCode).Select(x => x.LocalCode).Distinct().ToList())
                {
                    var _v = _dbContext.tblMdLocal.Find(v);
                    data.DB.Add(new DB
                    {
                        ColB = _v.Name,
                        IsBold = true,
                    });
                    foreach (var c in lstCustomer.Where(x => x.LocalCode == v && x.CustomerTypeCode == "KBM").ToList())
                    {

                        var _c = new DB
                        {
                            Code = c.Code,
                            ColA = _oDb.ToString(),
                            ColB = c.Name,
                            Col1 = lstMarket.FirstOrDefault(x => x.Code == c.MarketCode).Name,
                            Col2 = c.Gap,
                            Col4 = data.PT.Where(x => x.Code == c.MarketCode).Sum(x => x.Col4),
                            Col5 = c.CuocVcBq ?? 0,
                            Col6 = 0,
                            Col3 = data.PT.Where(x => x.Code == c.MarketCode).Sum(x => x.Col4 + x.Col6) + c.CuocVcBq,
                            Col8 = 0,
                            Col9 = c.MgglhXang,
                            Col10 = c.MgglhDau,

                        };
                        var _rPt = data.PT.FirstOrDefault(x => x.Code == c.MarketCode);
                        foreach (var g in lstGoods)
                        {
                            var _lg = c.LocalCode == "V1" ? data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "TT").Sum(x => x.Col13) : data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "OTHER").Sum(x => x.Col13);
                            _c.LG.Add(Math.Round(_lg ?? 0));
                            var vat = g.Type == "X" ? _rPt.GG.Where(x => x.Code == g.Code).Sum(x => x.VAT) + _c.Col9 + _c.Col8 : _rPt.GG.Where(x => x.Code == g.Code).Sum(x => x.VAT) + _c.Col10 + _c.Col8;
                            var nonVat = vat == 0 ? 0 : vat / 1.1M;
                            _c.GG.Add(new DB_GG
                            {
                                VAT = Math.Round(vat ?? 0),
                                NonVAT = Math.Round(nonVat ?? 0),
                            });

                            _c.LN.Add(Math.Round(_lg - _c.Col3 - nonVat - 0 / 1.1M ?? 0));


                            var bv = c.LocalCode == "V1" ? data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "TT").Sum(x => x.Col6) : data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "OTHER").Sum(x => x.Col6);
                            var t = c.LocalCode == "V1" ? data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "TT").Sum(x => x.Col2) : data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "OTHER").Sum(x => x.Col2);

                            var bv_vat = bv - vat;
                            var bv_nonVat = bv_vat != 0 ? bv_vat / 1.1M - t : 0;
                            _c.BVMT.Add(new DB_BVMT
                            {
                                Code = g.Code,
                                VAT = Math.Round(bv_vat ?? 0),
                                NonVAT = Math.Round(bv_nonVat ?? 0)
                            });


                        };


                        data.DB.Add(_c);
                        _oDb++;
                    }
                }
                #endregion

                #region FOB

                foreach (var l in lstCustomer.Where(x => x.CustomerTypeCode == "KBB").OrderBy(x => x.LocalCode).Select(x => x.LocalCode).Distinct().ToList())
                {
                    var local = _dbContext.tblMdLocal.Find(l);
                    data.FOB.Add(new FOB
                    {
                        ColB = local.Name,
                        IsBold = true,
                    });
                    var _oFob = 1;
                    foreach (var c in lstCustomer.Where(x => x.CustomerTypeCode == "KBB" && x.LocalCode == l))
                    {
                        var _fob = new FOB
                        {
                            Code = c.Code,
                            ColA = _oFob.ToString(),
                            ColB = c.Name,
                            Col2 = data.PT.Where(x => x.Code == c.MarketCode).Sum(x => x.Col4) ?? 0,
                            Col3 = c.CuocVcBq ?? 0,
                            Col4 = 0,
                            Col1 = c.CuocVcBq ?? 0 + data.PT.Where(x => x.Code == c.MarketCode).Sum(x => x.Col4),
                            Col5 = 0,
                            Col6 = c.MgglhXang ?? 0,
                            Col7 = c.MgglhDau ?? 0,
                            Col8 = 0,
                        };
                        _oFob++;
                        var lPT = data.PT.FirstOrDefault(x => x.Code == c.MarketCode);
                        foreach (var g in lstGoods)
                        {
                            var lg = l == "V1" ? data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "TT").Sum(x => x.Col13) : data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "OTHER").Sum(x => x.Col13);
                            _fob.LG.Add(Math.Round(lg ?? 0));

                            var gg = l == "V1" ? data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "TT").Sum(x => x.Col14) : data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "OTHER").Sum(x => x.Col14);
                            var vat = g.Type == "X" ? gg + _fob.Col6 + _fob.Col5 : gg + _fob.Col7 + _fob.Col5;
                            var nonVat = vat == 0 ? 0 : vat / 1.1M;

                            _fob.GG.Add(new FOB_GG
                            {
                                VAT = Math.Round(vat ?? 0),
                                NonVAT = Math.Round(nonVat ?? 0),
                            });
                            _fob.LN.Add(Math.Round(lg - _fob.Col1 - nonVat - _fob.Col8 / 1.1M ?? 0));


                            var bv = c.LocalCode == "V1" ? data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "TT").Sum(x => x.Col6) : data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "OTHER").Sum(x => x.Col6);
                            var t = c.LocalCode == "V1" ? data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "TT").Sum(x => x.Col2) : data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "OTHER").Sum(x => x.Col2);

                            var bv_vat = bv - vat;
                            var bv_nonVat = bv_vat != 0 ? bv_vat / 1.1M - t : 0;
                            _fob.BVMT.Add(new PT_BVMT
                            {
                                Code = g.Code,
                                VAT = Math.Round(bv_vat ?? 0),
                                NonVAT = Math.Round(bv_nonVat ?? 0)
                            });

                        }
                        data.FOB.Add(_fob);

                    }
                }

                #endregion

                #region PT09
                var _oPt09 = 1;
                foreach (var c in lstCustomer.Where(x => x.CustomerTypeCode == "TNPP").ToList())
                {
                    var _i = new PT09
                    {
                        Code = c.Code,
                        ColA = _oPt09.ToString(),
                        ColB = c.Name,
                        Col5 = 0,
                        Col6 = 0,
                        Col4 = lstMarket.FirstOrDefault()?.CPChungChuaCuocVC,
                        Col7 = c.MgglhXang,
                        Col8 = c.MgglhDau,
                        Col18 = 0,
                    };
                    _i.Col3 = _i.Col4 + _i.Col5 + _i.Col6;

                    var _pl4 = new PL4
                    {
                        Code = c.Code,
                        ColA = _oPt09.ToString(),
                        ColB = c.Name,
                    };
                    data.PL4.Add(_pl4);


                    foreach (var g in lstGoods)
                    {
                        var _2 = Math.Round(data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "OTHER").Sum(x => x.Col13) ?? 0);
                        _i.LG.Add(_2);

                        var _x_d = g.Type == "X" ? _i.Col7 : _i.Col8;
                        var vat = Math.Round(data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "OTHER").Sum(x => x.Col14) ?? 0) + _x_d;
                        var nonVat = vat != 0 ? vat / 1.1M : 0;
                        _i.GG.Add(new PT09_GG
                        {
                            VAT = Math.Round(vat ?? 0),
                            NonVAT = Math.Round(nonVat ?? 0),
                        });
                        _i.LN.Add(Math.Round(_2 - _i.Col3 - nonVat - _i.Col18 ?? 0));
                        _pl4.GG.Add(Math.Round(vat ?? 0));


                        var bv = data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "OTHER").Sum(x => x.Col6);
                        var t = data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "OTHER").Sum(x => x.Col2);

                        var bv_vat = bv - vat;
                        var bv_nonVat = bv_vat != 0 ? bv_vat / 1.1M - t : 0;
                        _i.BVMT.Add(new PT_BVMT
                        {
                            Code = g.Code,
                            VAT = Math.Round(bv_vat ?? 0),
                            NonVAT = Math.Round(bv_nonVat ?? 0)
                        });


                    }
                    _oPt09++;
                    data.PT09.Add(_i);

                }
                #endregion

                #region PL2
                var _oPl2 = 1;
                foreach (var c in lstCustomer.Where(x => x.CustomerTypeCode == "KBM").ToList())
                {
                    var pl2 = new PL2
                    {
                        Code = c.Code,
                        ColA = _oPl2.ToString(),
                        ColB = c.Name,
                    };
                    var _db = data.DB.FirstOrDefault(x => x.Code == c.Code);
                    _oPl2++;
                    if (_db == null) continue;
                    foreach (var gg in _db.GG)
                    {
                        pl2.GG.Add(gg.VAT);
                    }
                    data.PL2.Add(pl2);
                }

                #endregion

                #region PL3
                var _oPl3 = 1;
                foreach (var c in lstCustomer.Where(x => x.CustomerTypeCode == "KBB").ToList())
                {
                    var pl3 = new PL3
                    {
                        Code = c.Code,
                        ColA = _oPl3.ToString(),
                        ColB = c.Name,
                    };
                    var dataFob = data.FOB.FirstOrDefault(x => x.Code == c.Code);
                    _oPl3++;
                    if (dataFob == null) continue;
                    foreach (var gg in dataFob.GG)
                    {
                        pl3.GG.Add(gg.VAT);
                    }

                    data.PL3.Add(pl3);
                }

                #endregion

                #region VK11 PT
                foreach (var g in lstGoods)
                {
                    data.VK11PT.Add(new VK11PT
                    {
                        ColB = g.Name,
                        IsBold = true,
                    });
                    var _o = 1;
                    foreach (var c in lstCustomer.Where(x => x.CustomerTypeCode == "VK11PT").ToList())
                    {
                        var m = _dbContext.TblMdMarket.Find(c.MarketCode);
                        var v = data.PT.FirstOrDefault(x => x.Code == c.MarketCode).BVMT.Where(x => x.Code == g.Code).Sum(x => x.NonVAT);
                        var _i = new VK11PT
                        {
                            ColA = _o.ToString(),
                            ColB = c.Name,
                            Col1 = m?.Name,
                            Col2 = c.Gap,
                            Col4 = "10",
                            Col5 = c.Code,
                            Col6 = g.Code,
                            Col7 = "L",
                            Col9 = Math.Round(v ?? 0),
                            Col10 = "VND",
                            Col11 = 1,
                            Col12 = "L",
                            Col13 = "C"
                        };
                        data.VK11PT.Add(_i);
                        _o++;
                    }
                }
                #endregion

                #region VK11 ĐB
                foreach (var g in lstGoods)
                {
                    data.VK11DB.Add(new VK11DB
                    {
                        ColB = g.Name,
                        IsBold = true,
                    });
                    var _o = 1;
                    foreach (var c in lstCustomer.Where(x => x.CustomerTypeCode == "KBM").ToList())
                    {
                        var m = _dbContext.TblMdMarket.Find(c.MarketCode);
                        var v = data.DB.FirstOrDefault(x => x.Code == c.Code).BVMT.Where(x => x.Code == g.Code).Sum(x => x.NonVAT);
                        var _i = new VK11DB
                        {
                            ColA = _o.ToString(),
                            ColB = c.Address,
                            ColC = c.Name,
                            Col1 = m?.Name,
                            Col2 = c.Gap,
                            Col4 = "10",
                            Col5 = c.Code,
                            Col6 = g.Code,
                            Col7 = "L",
                            Col9 = Math.Round(v ?? 0),
                            Col10 = "VND",
                            Col11 = 1,
                            Col12 = "L",
                            Col13 = "C"
                        };
                        data.VK11DB.Add(_i);
                        _o++;
                    }
                }
                #endregion

                #region VK11 FOB
                foreach (var g in lstGoods)
                {
                    data.VK11FOB.Add(new VK11FOB
                    {
                        ColB = g.Name,
                        IsBold = true,
                    });
                    var _o = 1;
                    foreach (var c in lstCustomer.Where(x => x.CustomerTypeCode == "KBB").ToList())
                    {
                        var m = _dbContext.TblMdMarket.Find(c.MarketCode);
                        var v = data.FOB.FirstOrDefault(x => x.Code == c.Code).BVMT.Where(x => x.Code == g.Code).Sum(x => x.NonVAT);
                        var _i = new VK11FOB
                        {
                            ColA = _o.ToString(),
                            ColB = c.Address,
                            ColC = c.Name,
                            Col1 = m?.Name,
                            Col2 = c.Gap,
                            Col4 = "09",
                            Col5 = c.Code,
                            Col6 = g.Code,
                            Col7 = "L",
                            Col9 = Math.Round(v ?? 0),
                            Col10 = "VND",
                            Col11 = 1,
                            Col12 = "L",
                            Col13 = "C"
                        };
                        data.VK11FOB.Add(_i);
                        _o++;
                    }
                }
                #endregion

                #region VK11 TNPP
                foreach (var g in lstGoods)
                {
                    data.VK11TNPP.Add(new VK11TNPP
                    {
                        ColB = g.Name,
                        IsBold = true,
                    });
                    var _o = 1;
                    foreach (var c in lstCustomer.Where(x => x.CustomerTypeCode == "TNPP").ToList())
                    {
                        var m = _dbContext.TblMdMarket.Find(c.MarketCode);
                        var v = data.PT09.FirstOrDefault(x => x.Code == c.Code).BVMT.Where(x => x.Code == g.Code).Sum(x => x.NonVAT);
                        var _i = new VK11TNPP
                        {
                            ColA = _o.ToString(),
                            ColB = c.Address,
                            ColC = c.Name,
                            Col1 = m?.Name,
                            Col2 = c.Gap,
                            Col4 = "10",
                            Col5 = c.Code,
                            Col6 = g.Code,
                            Col7 = "L",
                            Col9 = Math.Round(v ?? 0),
                            Col10 = "VND",
                            Col11 = 1,
                            Col12 = "L",
                            Col13 = "C"
                        };
                        data.VK11TNPP.Add(_i);
                        _o++;
                    }
                }
                #endregion

                #region BBDO
                foreach (var g in lstGoods.OrderByDescending(x => x.CreateDate).ToList())
                {
                    var c = mappingBBDO.Where(x => x.GoodsCode == g.Code && x.Type == "BBDO").ToList();
                    if (c.Count() == 0) continue;
                    data.BBDO.Add(new BBDO
                    {
                        ColB = g.Name,
                        IsBold = true
                    });
                    var _m = new List<BBDO_MAP>();
                    foreach (var i in c)
                    {
                        _m.Add(new BBDO_MAP
                        {
                            CustomerCode = i.CustomerCode,
                            PointCode = i.DeliveryPointCode,
                            CustomerName = lstCustomer.FirstOrDefault(x => x.Code == i.CustomerCode)?.Name,
                            PointName = dataPoint.FirstOrDefault(x => x.Code == i.DeliveryPointCode)?.Name,
                        });
                    }
                    _m = _m.OrderBy(x => x.CustomerName).ThenBy(x => x.PointName).ToList();
                    var _o = 1;
                    foreach (var e in _m)
                    {
                        var i = new BBDO
                        {
                            ColA = _o.ToString(),
                            ColB = e.CustomerName,
                            ColC = e.PointName,
                            ColD = g.Name,
                            Col1 = "07",
                            Col2 = e.CustomerCode,
                            Col3 = g.Code,
                            Col4 = "L",
                            Col5 = lstCustomer.FirstOrDefault(x => x.Code == e.CustomerCode)?.PaymentTerm,
                            Col6 = Math.Round(data.DLG.Dlg_4.Where(x => x.Type == "OTHER" && x.Code == g.Code).Sum(x => x.Col12) ?? 0),
                            Col9 = data.PT.FirstOrDefault(x => x.IsBold == false)?.Col4,
                            Col10 = dataPoint.FirstOrDefault(x => x.Code == e.PointCode)?.CuocVcBq,
                            Col11 = lstCustomer.FirstOrDefault(x => x.Code == e.CustomerCode)?.BankLoanInterest,
                            Col12 = Math.Round(data.DLG.Dlg_4.Where(x => x.Type == "OTHER" && x.Code == g.Code).Sum(x => x.Col14) ?? 0),
                        };
                        i.Col8 = Math.Round(i.Col9 + i.Col10 + i.Col11 ?? 0);
                        i.Col7 = i.Col6 == 0 ? 0 : Math.Round(i.Col6 / 1.1M ?? 0);
                        i.Col13 = i.Col12 == 0 ? 0 : Math.Round(i.Col12 / 1.1M ?? 0);
                        i.Col14 = Math.Round(i.Col12 - i.Col10 * 1.1M ?? 0);
                        i.Col15 = i.Col14 == 0 ? 0 : Math.Round(i.Col14 / 1.1M ?? 0);
                        i.Col17 = Math.Round(i.Col7 - i.Col8 - i.Col15 - i.Col11 ?? 0);
                        i.Col16 = Math.Round(i.Col17 * 1.1M ?? 0);
                        i.Col19 = Math.Round(data.DLG.Dlg_4.Where(x => x.Type == "OTHER" && x.Code == g.Code).Sum(x => x.Col6) - i.Col14 ?? 0);
                        i.Col18 = i.Col19 == 0 ? 0 : Math.Round(i.Col19 / 1.1M - data.DLG.Dlg_4.Where(x => x.Type == "OTHER" && x.Code == g.Code).Sum(x => x.Col2) ?? 0);

                        data.BBDO.Add(i);
                        _o++;
                    }

                }
                #endregion

                #region DO FO
                var lstMapDOFO = mappingBBDO.Where(x => x.Type == "BBFO").ToList();
                var _oDOFO = 1;
                foreach (var _c in lstMapDOFO.Select(x => x.CustomerCode).ToList().Distinct().ToList())
                {
                    var g = lstGoods.FirstOrDefault(x => x.Code == lstMapDOFO.FirstOrDefault().GoodsCode);
                    var lstPointCode = lstMapDOFO.Where(x => x.CustomerCode == _c).Select(x => x.DeliveryPointCode).ToList();
                    var lstPoint = dataPoint.Where(x => lstPointCode.Contains(x.Code)).ToList();

                    data.BBFO.Add(new BBFO
                    {
                        ColA = _oDOFO.ToString(),
                        ColB = lstCustomer.FirstOrDefault(x => x.Code == _c)?.Name,
                        IsBold = true,
                    });
                    foreach (var p in lstPoint)
                    {
                        var _5 = lstMapDOFO.FirstOrDefault(x => x.CustomerCode == _c && x.DeliveryPointCode == p.Code);
                        var i = new BBFO
                        {
                            ColA = "-",
                            ColB = p.Name,
                            ColC = g.Name,
                            Col1 = Math.Round(data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "TT").Sum(x => x.Col8) ?? 0),
                            Col2 = Math.Round(data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "TT").Sum(x => x.Col9) ?? 0),
                            Col4 = data.PT.FirstOrDefault(x => x.IsBold == false)?.Col4,
                            Col5 = _5?.CuocVcBq,
                            Col6 = Math.Round(lstCustomer.FirstOrDefault(x => x.Code == _c)?.BankLoanInterest ?? 0),
                        };
                        i.Col3 = Math.Round(i.Col4 + i.Col5 + i.Col6 ?? 0);
                        var _8 = data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "TT").Sum(x => x.Col7);
                        i.Col8 = _8 == 0 ? 0 : Math.Round(_8 / 100 ?? 0);
                        i.Col7 = Math.Round(i.Col8 + i.Col3 - i.Col2 ?? 0);
                        i.Col10 = Math.Round(data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "TT").Sum(x => x.Col6) + i.Col7 ?? 0);
                        i.Col9 = i.Col10 == 0 ? 0 : Math.Round(i.Col10 / 1.1M - data.DLG.Dlg_4.Where(x => x.Code == g.Code && x.Type == "TT").Sum(x => x.Col2) ?? 0);


                        data.BBFO.Add(i);
                    }

                    _oDOFO++;
                }
                #endregion

                #region VK11 BB
                foreach (var i in data.BBDO)
                {
                    data.VK11BB.Add(new VK11BB
                    {
                        ColA = i.ColA,
                        ColB = i.ColB,
                        ColC = i.ColC,
                        ColD = i.ColD,
                        Col1 = i.Col1,
                        Col2 = i.Col2,
                        Col3 = i.Col3,
                        Col4 = i.Col4,
                        Col5 = i.Col5,
                        Col6 = i.Col19,
                        Col7 = "VND",
                        Col8 = "1",
                        Col9 = "L",
                        Col10 = "C",
                        IsBold = i.IsBold,
                    });
                }
                #endregion

                #region Tổng hợp
                data.Summary.Add(new Summary
                {
                    Code = Guid.NewGuid().ToString(),
                    ColB = "TNQTM",
                    IsBold = true,
                });
                foreach (var i in data.VK11PT)
                {
                    data.Summary.Add(new Summary
                    {
                        Code = Guid.NewGuid().ToString(),
                        ColA = i.ColA,
                        ColB = i.ColB,
                        ColC = i.Col1,
                        Col1 = i.Col4,
                        Col2 = i.Col5,
                        Col3 = i.Col6,
                        Col4 = i.Col7,
                        Col5 = i.Col8,
                        Col6 = i.Col9,
                        Col7 = "VND",
                        Col8 = "1",
                        Col9 = "L",
                        Col10 = "C",
                        IsBold = i.IsBold,
                    });
                }

                data.Summary.Add(new Summary
                {
                    Code = Guid.NewGuid().ToString(),
                    ColB = "KHÁCH ĐẶC BIỆT",
                    IsBold = true,
                });
                foreach (var i in data.VK11DB)
                {
                    data.Summary.Add(new Summary
                    {
                        Code = Guid.NewGuid().ToString(),
                        ColA = i.ColA,
                        ColB = i.ColB,
                        ColC = i.ColC,
                        ColD = i.Col1,
                        Col1 = i.Col4,
                        Col2 = i.Col5,
                        Col3 = i.Col6,
                        Col4 = i.Col7,
                        Col5 = i.Col8,
                        Col6 = i.Col9,
                        Col7 = "VND",
                        Col8 = "1",
                        Col9 = "L",
                        Col10 = "C",
                        IsBold = i.IsBold,
                    });
                }

                data.Summary.Add(new Summary
                {
                    Code = Guid.NewGuid().ToString(),
                    ColB = "BÁN FOB",
                    IsBold = true,
                });
                foreach (var i in data.VK11FOB)
                {
                    data.Summary.Add(new Summary
                    {
                        Code = Guid.NewGuid().ToString(),
                        ColA = i.ColA,
                        ColB = i.IsBold ? i.ColB : i.ColC,
                        ColC = i.Col1,
                        Col1 = i.Col4,
                        Col2 = i.Col5,
                        Col3 = i.Col6,
                        Col4 = i.Col7,
                        Col5 = i.Col8,
                        Col6 = i.Col9,
                        Col7 = "VND",
                        Col8 = "1",
                        Col9 = "L",
                        Col10 = "C",
                        IsBold = i.IsBold,
                    });
                }

                data.Summary.Add(new Summary
                {
                    Code = Guid.NewGuid().ToString(),
                    ColB = "TNPP",
                    IsBold = true,
                });
                foreach (var i in data.VK11TNPP)
                {
                    data.Summary.Add(new Summary
                    {
                        Code = Guid.NewGuid().ToString(),
                        ColA = i.ColA,
                        ColB = i.IsBold ? i.ColB : i.ColC,
                        ColC = i.Col1,
                        Col1 = i.Col4,
                        Col2 = i.Col5,
                        Col3 = i.Col6,
                        Col4 = i.Col7,
                        Col5 = i.Col8,
                        Col6 = i.Col9,
                        Col7 = "VND",
                        Col8 = "1",
                        Col9 = "L",
                        Col10 = "C",
                        IsBold = i.IsBold,
                    });
                }

                data.Summary.Add(new Summary
                {
                    Code = Guid.NewGuid().ToString(),
                    ColB = "BÁN BUÔN",
                    IsBold = true,
                });
                foreach (var i in data.VK11BB)
                {
                    data.Summary.Add(new Summary
                    {
                        Code = Guid.NewGuid().ToString(),
                        ColA = i.ColA,
                        ColB = i.ColB,
                        ColC = i.ColC,
                        ColD = i.ColD,
                        Col1 = i.Col1,
                        Col2 = i.Col2,
                        Col3 = i.Col3,
                        Col4 = i.Col4,
                        Col5 = i.Col5,
                        Col6 = i.Col6,
                        Col7 = "VND",
                        Col8 = "1",
                        Col9 = "L",
                        Col10 = "C",
                        IsBold = i.IsBold,
                    });
                }
                #endregion

                return await RoundNumberData(data);
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return new CalculateResultModel();
            }
        }

        public async Task<CalculateResultModel> RoundNumberData(CalculateResultModel data)
        {
            try
            {
                foreach (var i in data.DLG.Dlg_3)
                {
                    i.Col4 = Math.Round(i.Col4 ?? 0);
                    i.Col5 = Math.Round(i.Col5 ?? 0);
                    i.Col8 = Math.Round(i.Col8 ?? 0);
                    i.Col9 = Math.Round(i.Col9 ?? 0);
                    i.Col10 = Math.Round(i.Col10 ?? 0);
                }
                foreach (var i in data.DLG.Dlg_5)
                {
                    i.Col4 = Math.Round(i.Col4 ?? 0);
                    i.Col5 = Math.Round(i.Col5 ?? 0);
                }
                foreach (var i in data.DLG.Dlg_6)
                {
                    i.Col3 = Math.Round(i.Col3 ?? 0);
                    i.Col6 = Math.Round(i.Col6 ?? 0);
                    i.Col7 = Math.Round(i.Col7 ?? 0);
                }
                foreach (var i in data.DLG.Dlg_4)
                {
                    i.Col4 = Math.Round(i.Col4 ?? 0);
                    i.Col5 = Math.Round(i.Col5 ?? 0);
                    i.Col8 = Math.Round(i.Col8 ?? 0);
                    i.Col9 = Math.Round(i.Col9 ?? 0);
                    i.Col10 = Math.Round(i.Col10 ?? 0);
                    i.Col7 = Math.Round(i.Col7 ?? 0);
                    i.Col12 = Math.Round(i.Col12 ?? 0);
                    i.Col13 = Math.Round(i.Col13 ?? 0);
                    i.Col15 = Math.Round(i.Col15 ?? 0);
                    i.Col16 = Math.Round(i.Col16 ?? 0);
                }
                return data;
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return new CalculateResultModel();
            }
        }

        public async Task<InsertModel> GetDataInput(string code)
        {
            try
            {
                var data = new InsertModel();
                data.Header = await _dbContext.TblBuCalculateResultList.FindAsync(code);
                data.HS1 = await _dbContext.TblInHeSoMatHang.Where(x => x.HeaderCode == code).ToListAsync();
                data.HS2 = await _dbContext.TblInVinhCuaLo.Where(x => x.HeaderCode == code).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return new InsertModel();
            }
        }

        public async Task UpdateDataInput(InsertModel model)
        {
            try
            {
                _dbContext.TblInHeSoMatHang.UpdateRange(model.HS1);
                _dbContext.TblInVinhCuaLo.UpdateRange(model.HS2);
                if (model.Header.Status == model.Status.Code)
                {
                    model.Header.Status = "01";
                    _dbContext.TblBuCalculateResultList.Update(model.Header);
                    var h = new TblBuHistoryAction()
                    {
                        Code = Guid.NewGuid().ToString(),
                        HeaderCode = model.Header.Code,
                        Action = "Cập nhật thông tin",
                    };
                }
                else
                {
                    model.Header.Status = model.Status.Code;
                    _dbContext.TblBuCalculateResultList.Update(model.Header);
                    var h = new TblBuHistoryAction()
                    {
                        Code = Guid.NewGuid().ToString(),
                        HeaderCode = model.Header.Code,
                        Action = model.Status.Code == "02" ? "Trình duyệt" : model.Status.Code == "03" ? "Yêu cầu chỉnh sửa" : model.Status.Code == "04" ? "Phê duyệt" : "Từ chối",
                        Contents = model.Status.Contents
                    };
                    _dbContext.TblBuHistoryAction.Add(h);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
            }
        }

        public async Task<List<TblBuHistoryAction>> GetHistoryAction(string code)
        {
            try
            {
                var data = await _dbContext.TblBuHistoryAction.Where(x => x.HeaderCode == code).OrderByDescending(x => x.CreateDate).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return new List<TblBuHistoryAction>();
            }
        }

        public void ExportExcel(ref MemoryStream outFileStream, string path, string headerId)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                IWorkbook templateWorkbook;
                templateWorkbook = new XSSFWorkbook(fs);
                fs.Close();


                var startRowPT = 7;
                ISheet sheetPT = templateWorkbook.GetSheetAt(1);

                //Define Style
                var styleCellNumber = GetCellStyleNumber(templateWorkbook);

                var font = templateWorkbook.CreateFont();
                font.FontHeightInPoints = 12;
                font.FontName = "Times New Roman";

                ICellStyle styleCellBold = templateWorkbook.CreateCellStyle(); // chữ in đậm
                styleCellBold.CloneStyleFrom(sheetPT.GetRow(1).Cells[0].CellStyle);
                var fontBold = templateWorkbook.CreateFont();
                fontBold.Boldweight = (short)FontBoldWeight.Bold;
                fontBold.FontHeightInPoints = 12;
                fontBold.FontName = "Times New Roman";

                //Get Data
                var data = GetResult(headerId);
                #region Export PT
                for (var i = 0; i < data.Result.PT.Count(); i++)
                {
                    var dataRow = data.Result.PT[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetPT, startRowPT++, 38);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);
                    rowCur.Cells[1].SetCellValue(dataRow.ColB);
                    rowCur.Cells[2].CellStyle = styleCellNumber;
                    rowCur.Cells[2].SetCellValue(dataRow.Col1 == 0 ? 0 : Convert.ToDouble(dataRow.Col1));


                    var iLG = 0;
                    for (var lg = iLG; lg < dataRow.LG.Count(); lg++)
                    {
                        rowCur.Cells[3 + lg].CellStyle = styleCellNumber;
                        rowCur.Cells[3 + lg].SetCellValue(dataRow.LG[lg] == 0 ? 0 : Convert.ToDouble(dataRow.LG[lg]));
                    }

                    rowCur.Cells[8].CellStyle = styleCellNumber;
                    rowCur.Cells[8].SetCellValue(dataRow.Col3 == 0 ? 0 : Convert.ToDouble(dataRow.Col3));
                    rowCur.Cells[9].CellStyle = styleCellNumber;
                    rowCur.Cells[9].SetCellValue(dataRow.Col4 == 0 ? 0 : Convert.ToDouble(dataRow.Col4));
                    rowCur.Cells[10].CellStyle = styleCellNumber;
                    rowCur.Cells[10].SetCellValue(dataRow.Col5 == 0 ? 0 : Convert.ToDouble(dataRow.Col5));

                    var iGG = 0;
                    for (var gg = 0; gg < dataRow.GG.Count(); gg++)
                    {
                        rowCur.Cells[13 + iGG].CellStyle = styleCellNumber;
                        rowCur.Cells[13 + iGG].SetCellValue(dataRow.GG[gg].VAT == 0 ? 0 : Convert.ToDouble(dataRow.GG[gg].VAT));
                        rowCur.Cells[14 + iGG].CellStyle = styleCellNumber;
                        rowCur.Cells[14 + iGG].SetCellValue(dataRow.GG[gg].NonVAT == 0 ? 0 : Convert.ToDouble(dataRow.GG[gg].NonVAT));
                        iGG += 2;
                    }

                    var iLN = 0;
                    for (var ln = iLN; ln < dataRow.LG.Count(); ln++)
                    {
                        rowCur.Cells[23 + ln].CellStyle = styleCellNumber;
                        rowCur.Cells[23 + ln].SetCellValue(dataRow.LN[ln] == 0 ? 0 : Convert.ToDouble(dataRow.LN[ln]));
                    }

                    var iBV = 0;
                    for (var gg = 0; gg < dataRow.BVMT.Count(); gg++)
                    {
                        rowCur.Cells[28 + iBV].CellStyle = styleCellNumber;
                        rowCur.Cells[28 + iBV].SetCellValue(dataRow.BVMT[gg].NonVAT == 0 ? 0 : Convert.ToDouble(dataRow.BVMT[gg].NonVAT));
                        rowCur.Cells[29 + iBV].CellStyle = styleCellNumber;
                        rowCur.Cells[29 + iBV].SetCellValue(dataRow.BVMT[gg].VAT == 0 ? 0 : Convert.ToDouble(dataRow.BVMT[gg].VAT));
                        iBV += 2;
                    }

                    for (var j = 0; j < 38; j++)
                    {
                        if (dataRow.IsBold)
                        {
                            rowCur.Cells[j].CellStyle = styleCellBold;
                            rowCur.Cells[j].CellStyle.SetFont(fontBold);
                        }
                        else
                        {
                            rowCur.Cells[j].CellStyle.SetFont(font);
                        }
                        rowCur.Cells[j].CellStyle.BorderBottom = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderTop = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderLeft = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderRight = BorderStyle.Thin;
                    }
                }


                #endregion

                #region Export ĐB
                #endregion

                templateWorkbook.Write(outFileStream);
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
            }
        }
        public ICellStyle GetCellStyleNumber(IWorkbook templateWorkbook)
        {
            ICellStyle styleCellNumber = templateWorkbook.CreateCellStyle();
            styleCellNumber.DataFormat = templateWorkbook.CreateDataFormat().GetFormat("#,##0");
            return styleCellNumber;
        }
        public ICellStyle GetCellStyleNumberDecimal(IWorkbook templateWorkbook)
        {
            ICellStyle styleCellNumber = templateWorkbook.CreateCellStyle();
            styleCellNumber.DataFormat = templateWorkbook.CreateDataFormat().GetFormat("#,##0.000");
            return styleCellNumber;
        }
        public ICellStyle GetCellStyleNumberDecimal2(IWorkbook templateWorkbook)
        {
            ICellStyle styleCellNumber = templateWorkbook.CreateCellStyle();
            styleCellNumber.DataFormat = templateWorkbook.CreateDataFormat().GetFormat("#,###.#0");
            return styleCellNumber;
        }
        public ICellStyle GetCellStylePercentage(IWorkbook templateWorkbook)
        {
            ICellStyle styleCellPercentage = templateWorkbook.CreateCellStyle();
            styleCellPercentage.DataFormat = templateWorkbook.CreateDataFormat().GetFormat("0.000%");
            return styleCellPercentage;
        }

    }
}
