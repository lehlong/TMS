﻿using AutoMapper;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.BU;
using DMS.CORE.Entities.BU;
using DMS.CORE;

using DMS.CORE.Entities.MD;
using DMS.BUSINESS.Dtos.MD;
using Microsoft.AspNetCore.Http;
using DMS.BUSINESS.Models;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SMO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using PROJECT.Service.Extention;
using Aspose.Words;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using DocumentFormat.OpenXml;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using System.Linq;
using NPOI.HPSF;
using NPOI.SS.Formula.Functions;
using DMS.CORE.Entities.IN;
using Aspose.Words.Tables;

namespace DMS.BUSINESS.Services.BU
{
    public interface ICalculateResultService : IGenericService<TblMdGoods, GoodsDto>
    {
        Task<CalculateResultModel> GetResult(string code);
        Task<InsertModel> GetDataInput(string code);
        Task<List<TblBuHistoryAction>> GetHistoryAction(string code);
        Task<List<TblBuHistoryDownload>> GetHistoryFile(string code);
        Task<List<TblMdCustomer>> GetCustomer();
        Task UpdateDataInput(InsertModel model);
        void ExportExcel(ref MemoryStream outFileStream, string path, string headerId);
        Task<string> SaveFileHistory(MemoryStream outFileStream, string headerId);
        Task<string> GenarateWordTrinhKy(string headerId, string nameTeam);
        Task<string> GenarateWord(List<string> lstCustomerChecked, string headerId);
        Task<string> GenarateFile(List<string> lstCustomerChecked, string type, string headerId);
        Task<string> ExportExcelTrinhKy(string headerId);
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
                //var lstLGDT = await _dbContext.TblMdLaiGopDieuTiet.ToListAsync();
                var lstCustomer = await _dbContext.TblMdCustomer.ToListAsync();
                var lstCR = await _dbContext.TblBuCalculateResultList.OrderBy(x => x.FDate).ToListAsync();
                
                DateTime fDate = lstCR.FirstOrDefault(x => x.Code == code).FDate;

                //var CodeOldCalculate = await _dbContext.TblBuCalculateResultList.Where();
                var CodeOldCalculate = await _dbContext.TblBuCalculateResultList
                                            .Where(x => x.FDate < fDate) // Lọc các trường có FDate nhỏ hơn ngày hiện tại
                                            .Where(x => x.Status == "04") //lấy trường đã đươc phê duyệt
                                            .OrderByDescending(x => x.FDate)
                                            .Select(x => x.Code) // Chọn trường Code
                                            .FirstOrDefaultAsync();

                var dataVCL = await _dbContext.TblInVinhCuaLo.Where(x => x.HeaderCode == code).ToListAsync();
                var dataVCLOld = await _dbContext.TblInVinhCuaLo.Where(x => x.HeaderCode == CodeOldCalculate).ToListAsync();
                var dataHSMH = await _dbContext.TblInHeSoMatHang.Where(x => x.HeaderCode == code).ToListAsync();
                var dataHSMHOld = await _dbContext.TblInHeSoMatHang.Where(x => x.HeaderCode == CodeOldCalculate).ToListAsync();
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
                        Col10 = hsmh.Sum(x => x.LaiGopDieuTiet) == null ? 0 : hsmh.Sum(x => x.LaiGopDieuTiet),
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
                        Col10 = hsmh.Sum(x => x.LaiGopDieuTiet) == null ? 0 : hsmh.Sum(x => x.LaiGopDieuTiet),
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

                #region

                
                foreach (var g in lstGoods)
                {
                    var hsmho = dataHSMHOld.Where(x => x.GoodsCode == g.Code).ToList();
                    var dlg1 = data.DLG.Dlg_1.Where(x => x.Code == g.Code).ToList();
                    var k = new DLG_4_Old
                    {
                        Code = g.Code,
                        Type = "TT",
                        ColB = g.Name,
                        Col1 = hsmho.Sum(x => x.HeSoVcf),
                        Col2 = hsmho.Sum(x => x.ThueBvmt),
                        Col3 = hsmho.Sum(x => x.L15ChuaVatBvmtNbl),
                        Col4 = hsmho.Sum(x => x.HeSoVcf) * hsmho.Sum(x => x.L15ChuaVatBvmtNbl),
                        Col5 = (hsmho.Sum(x => x.ThueBvmt) + hsmho.Sum(x => x.HeSoVcf) * hsmho.Sum(x => x.L15ChuaVatBvmtNbl)) * 1.1M,
                        Col6 = dlg1.Sum(x => x.Col6),
                        Col14 = hsmho.Sum(x => x.GiamGiaFob),
                        Col10 = hsmho.Sum(x => x.LaiGopDieuTiet) == null ? 0 : hsmho.Sum(x => x.LaiGopDieuTiet),
                    };
                    if (k.Col6 != 0)
                    {
                       k.Col7 = k.Col6 / 1.1M - k.Col2;
                    }
                    k.Col8 = k.Col6 - k.Col5;
                    if (k.Col8 != 0)
                    {
                        k.Col9 = k.Col8 / 1.1M;
                    }
                    k.Col11 = k.Col1 * k.Col10 * 1.1M;
                    k.Col13 = k.Col11 + k.Col9;
                    k.Col12 = k.Col13 * 1.1M;
                    k.Col15 = (k.Col12 - k.Col14) * k.Col1;
                    k.Col16 = k.Col12 - k.Col14;
                    data.DLG.Dlg_4_Old.Add(k);
                }

                foreach (var g in lstGoods)
                {
                    var hsmho = dataHSMHOld.Where(x => x.GoodsCode == g.Code).ToList();
                    var dlg1 = data.DLG.Dlg_3.Where(x => x.Code == g.Code).ToList();
                    var k = new DLG_4_Old
                    {
                        Code = g.Code,
                        Type = "OTHER",
                        ColA = _oII.ToString(),
                        ColB = g.Name,
                        Col1 = hsmho.Sum(x => x.HeSoVcf),
                        Col2 = hsmho.Sum(x => x.ThueBvmt),
                        Col3 = hsmho.Sum(x => x.L15ChuaVatBvmtNbl),
                        Col4 = hsmho.Sum(x => x.HeSoVcf) * hsmho.Sum(x => x.L15ChuaVatBvmtNbl),
                        Col5 = (hsmho.Sum(x => x.ThueBvmt) + hsmho.Sum(x => x.HeSoVcf) * hsmho.Sum(x => x.L15ChuaVatBvmtNbl)) * 1.1M,
                        Col6 = dlg1.Sum(x => x.Col7),
                        Col14 = hsmho.Sum(x => x.GiamGiaFob),
                        Col10 = hsmho.Sum(x => x.LaiGopDieuTiet) == null ? 0 : hsmho.Sum(x => x.LaiGopDieuTiet),
                    };
                    if (k.Col6 != 0)
                    {
                        k.Col7 = k.Col6 / 1.1M - k.Col2;
                    }
                    k.Col8 = k.Col6 - k.Col5;
                    if (k.Col8 != 0)
                    {
                        k.Col9 = k.Col8 / 1.1M;
                    }
                    k.Col11 = k.Col1 * k.Col10 * 1.1M;
                    k.Col13 = k.Col11 + k.Col9;
                    k.Col12 = k.Col13 * 1.1M;
                    k.Col15 = (k.Col12 -k.Col14) * k.Col1;
                    k.Col16 = k.Col12 - k.Col14;
                    data.DLG.Dlg_4_Old.Add(k);

                }
                #endregion


                // thay dổi giá bán lẻ 
                foreach (var g in lstGoods)
                {
                    var vcl = dataVCLOld.Where(x => x.GoodsCode == g.Code).ToList();
                    //var hsmh = dataHSMH.Where(x => x.GoodsCode == g.Code).ToList();
                    var dlg_1 = data.DLG.Dlg_1;
                    foreach (var n in dlg_1)
                    {
                        if (g.Code == n.Code)
                        {
                            var i = new Dlg_TDGBL
                            {
                                Code = g.Code,
                                ColA = g.Name,
                                Col1 = vcl.Sum(x => x.Gny), // lấy giá niêm yết ở kì trước
                                Col2 = n.Col6,
                                TangGiam1_2 = n.Col6 - vcl.Sum(x => x.Gny),
                                Col3 = vcl.Sum(x => x.GblV2), // lấy giá niêm yết ở kì trước
                                Col4 = n.Col3,
                                TangGiam3_4 = n.Col3 - vcl.Sum(x => x.GblV2),
                            };

                            data.DLG.Dlg_TDGBL.Add(i);
                        }

                    }
                }
                // Lãi gộp
                foreach (var g in lstGoods)
                {
                    var hsmh = dataHSMH.Where(x => x.GoodsCode == g.Code).ToList();
                    var hsmho = dataHSMHOld.Where(x => x.GoodsCode == g.Code).ToList();
                    var dlg_4 = data.DLG.Dlg_4;
                    var dlg_4_Old = data.DLG.Dlg_4_Old;
                    foreach (var n in dlg_4)
                    {
                        if (g.Code == n.Code)
                        {
                            var i = new DLG_7
                            {
                                Code = g.Code,
                                ColA = g.Name,
                                Type = n.Type,
                                Col1 = dlg_4_Old.Where(x => x.Code == g.Code).Where(x => x.Type == n.Type).Sum(x => x.Col12),
                                Col2 = n.Col12,
                                TangGiam1_2 = n.Col12 - dlg_4_Old.Where(x => x.Code == g.Code).Where(x => x.Type == n.Type).Sum(x => x.Col12),
                            };

                            data.DLG.Dlg_7.Add(i);
                        }
                    }
                }


                // Đề xuất mức giảm giá
                foreach (var g in lstGoods)
                {
                    var hsmh = dataHSMHOld.Where(x => x.GoodsCode == g.Code).ToList();
                    var dlg_4 = data.DLG.Dlg_4;
                    var dlg_4_Old = data.DLG.Dlg_4_Old;

                    foreach (var n in dlg_4)
                    {
                        if (g.Code == n.Code && n.Type == "TT")
                        {
                            var i = new DLG_8
                            {
                                Code = g.Code,
                                ColA = g.Name,
                                Type = n.Type,
                                Col1 = dlg_4_Old.Where(x => x.Code  == g.Code).Where(x => x.Type == "TT").Sum(x => x.Col14), 
                                Col2 = n.Col14,
                                TangGiam1_2 = n.Col14 - dlg_4_Old.Where(x => x.Code == g.Code).Where(x => x.Type == "TT").Sum(x => x.Col14),
                            };

                            data.DLG.Dlg_8.Add(i);
                        }

                    }
                }

                // thay đổi giá giao phương thức bán lẻ
                foreach (var g in lstGoods)
                {
                    var hsmh = dataHSMHOld.Where(x => x.GoodsCode == g.Code).ToList();
                    //var hsmh = dataHSMH.Where(x => x.GoodsCode == g.Code).ToList();
                    var dlg_3 = data.DLG.Dlg_3;
                    foreach (var n in dlg_3)
                    {
                        if (g.Code == n.Code)
                        {
                            var i = new Dlg_TdGgptbl
                            {
                                Code = g.Code,
                                ColA = g.Name,
                                Col1 = hsmh.Sum(x => x.L15ChuaVatBvmt), // lấy giá niêm yết ở kì trước
                                Col2 = n.Col3,
                                TangGiam1_2 = n.Col3 - hsmh.Sum(x => x.L15ChuaVatBvmt),
                            };

                            data.DLG.Dlg_TdGgptbl.Add(i);
                        }

                    }
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
                                //VAT = p - i.Col5 * m.Coefficient + i.Col6
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
                            //var vat = g.Type == "X" ? _rPt.GG.Where(x => x.Code == g.Code).Sum(x => x.VAT) + _c.Col9 + _c.Col8 : _rPt.GG.Where(x => x.Code == g.Code).Sum(x => x.VAT) + _c.Col10 + _c.Col8;
                            var vat = g.Type == "X"
                                            ? _rPt.GG.Where(x => x.Code == g.Code).Sum(x => x.VAT) + (g.Code == "601005" ? 0 : _c.Col9 + _c.Col8)
                                            : _rPt.GG.Where(x => x.Code == g.Code).Sum(x => x.VAT) + (g.Code == "601005" ? 0 : _c.Col10 + _c.Col8);

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
                        //i.Col19 = Math.Round(data.DLG.Dlg_4.Where(x => x.Type == "OTHER" && x.Code == g.Code).Sum(x => x.Col6) - i.Col14  ?? 0);
                      
                        i.Col19 =  (e.CustomerCode== "305152" ||e.CustomerCode== "308417") ? ((data.DLG.Dlg_4.Where(x => x.Type == "OTHER" && x.Code == g.Code).Sum(x => x.Col6) - i.Col16)==0 ? 0 : (int)((Math.Floor((data.DLG.Dlg_4.Where(x => x.Type == "OTHER" && x.Code == g.Code).Sum(x => x.Col6) - i.Col16 ?? 0)- (decimal)0.5) -5)/100)*100) :(data.DLG.Dlg_4.Where(x => x.Type == "OTHER" && x.Code == g.Code).Sum(x => x.Col6) - i.Col16 ??0) ;

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
                var nguoiKyTen = new TblInNguoiKyTen();
                nguoiKyTen.Code = Guid.NewGuid().ToString();
                nguoiKyTen.HeaderCode = code;
                nguoiKyTen.NguoiDaiDien = "";
                nguoiKyTen.DaiDien = "";
                nguoiKyTen.QuyetDinhSo = "";

                var data = new InsertModel();
                //data.NguoiKyTen = await _dbContext.TblInNguoiKyTen.FindAsync(code) == null ? nguoiKyTen : await _dbContext.TblInNguoiKyTen.FindAsync(code);
                var checkNull = await _dbContext.TblInNguoiKyTen.FirstOrDefaultAsync(x => x.HeaderCode == code);

                if (checkNull == null)
                {
                    data.NguoiKyTen = nguoiKyTen;
                }
                else
                {
                    data.NguoiKyTen = await _dbContext.TblInNguoiKyTen.FirstOrDefaultAsync(x => x.HeaderCode == code);
                }

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
                var checkNull = await _dbContext.TblInNguoiKyTen.FirstOrDefaultAsync(x => x.Code == model.NguoiKyTen.Code);
                //var checkNull = await _dbContext.TblInNguoiKyTen.FindAsync(model.NguoiKyTen.Code);
                if (checkNull == null)
                {
                    _dbContext.TblInNguoiKyTen.Add(model.NguoiKyTen);
                }
                else
                {
                    //checkNull = null;
                    checkNull.NguoiDaiDien = model.NguoiKyTen.NguoiDaiDien;
                    checkNull.DaiDien = model.NguoiKyTen.DaiDien;
                    checkNull.QuyetDinhSo = model.NguoiKyTen.QuyetDinhSo;

                    //_dbContext.TblInNguoiKyTen.UpdateRange(model.NguoiKyTen);
                }

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
                    model.Header.Status = model.Status.Code == "06" ? "01" : model.Status.Code == "07" ? "01" : model.Status.Code;
                    _dbContext.TblBuCalculateResultList.Update(model.Header);
                    var h = new TblBuHistoryAction()
                    {
                        Code = Guid.NewGuid().ToString(),
                        HeaderCode = model.Header.Code,
                        Action = model.Status.Code == "02" ? "Trình duyệt" : model.Status.Code == "03" ? "Yêu cầu chỉnh sửa" : model.Status.Code == "04" ? "Phê duyệt" : model.Status.Code == "05" ? "Từ chối" : model.Status.Code == "06" ? "Hủy trình duyệt" : "Hủy phê duyệt",
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
        
        public async Task<List<TblBuHistoryDownload>> GetHistoryFile(string code)
        {
            try
            {
                var data = await _dbContext.TblBuHistoryDownload.Where(x => x.HeaderCode == code).OrderByDescending(x => x.CreateDate).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return new List<TblBuHistoryDownload>();
            }
        }
        
        public async Task<List<TblMdCustomer>> GetCustomer()
        {
            try
            {
                var data = await _dbContext.TblMdCustomer.Where(x => x.CustomerTypeCode == "BBDO").OrderBy(x => x.Name).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return new List<TblMdCustomer>();
            }
        }
        public async Task<string> ExportExcelTrinhKy(string headerId)
        {
            try
            {

                var data = await GetResult(headerId);
                var header = await _dbContext.TblBuCalculateResultList.FindAsync(headerId);
                var model = await GetDataInput(headerId);
                var goods = await _dbContext.TblMdGoods.ToListAsync();
                var NguoiKyTen = await _dbContext.TblInNguoiKyTen.FirstOrDefaultAsync(x => x.HeaderCode == headerId);
                var A5 = $"  (Kèm theo Công văn số:                        /PLXNA ngày {header.FDate.Day:D2}/{header.FDate.Month:D2}/{header.FDate.Year} của Công ty Xăng dầu Nghệ An)";
                var A24 = $" + Căn cứ Quyết định số {NguoiKyTen.QuyetDinhSo} ngày {header.FDate.Day:D2}/{header.FDate.Month:D2}/{header.FDate.Year} của Tổng giám đốc Tập đoàn Xăng dầu Việt Nam về việc qui định giá bán xăng dầu; ";
                var B25 = $"Mức giá bán đăng ký này có hiệu lực thi hành kể từ 15 giờ 00 ngày {header.FDate.Day} tháng {header.FDate.Month} năm {header.FDate.Year}";
                // 1. Đường dẫn file gốc
                var filePathTemplate = Path.Combine(Directory.GetCurrentDirectory(), "Template", "TempTrinhKy", "KeKhaiGiaChiTiet.xlsx");

                // 2. Tạo thư mục lưu file
                var folderName = Path.Combine($"Upload/{DateTime.Now.Year}/{DateTime.Now.Month}");
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }

                // 3. Tạo tên file mới
                var fileName = $"{DateTime.Now:ddMMyyyy_HHmmss}_KeKhaiGiaChiTiet.xlsx";
                var fullPath = Path.Combine(folderName, fileName);

                // 4. Copy file từ Template sang Upload
                File.Copy(filePathTemplate, fullPath, true);

                // 5. Mở file để sửa
                IWorkbook workbook;

                using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite))
                {
                    workbook = new XSSFWorkbook(fs);
                    ISheet sheet = workbook.GetSheetAt(0);
                    IRow rowA5 = sheet.GetRow(4);
                    ICell cellA5 = rowA5?.GetCell(0);

                    if (cellA5 != null)
                    {
                        cellA5.SetCellValue(A5);
                    }

                    IRow rowA24 = sheet.GetRow(23);
                    ICell cellA24 = rowA24?.GetCell(0);

                    if (cellA24 != null)
                    {
                        cellA24.SetCellValue(A24);
                    }

                    IRow rowB25 = sheet.GetRow(24);
                    ICell cellB25 = rowB25?.GetCell(1);

                    if (cellB25 != null)
                    {
                        cellB25.SetCellValue(B25);
                    }
                    int rowIndex = 10; // Bắt đầu từ row 11 (index = 10)
                    foreach (var item in data.DLG.Dlg_TDGBL)
                    {
                        IRow row = sheet.GetRow(rowIndex); // Chỉ lấy row, không cần CreateRow
                            if (row != null && !item.Code.Trim().Equals("701001", StringComparison.OrdinalIgnoreCase))
                         {
                            // B11 -> colA
                            ICell cellB = row.GetCell(1);
                            if (cellB != null)
                            {
                                cellB.SetCellValue(item.ColA);
                            }

                            // E11 -> col1
                            ICell cellE = row.GetCell(4);
                            if (cellE != null && item.Col1.HasValue)
                            {
                                cellE.SetCellValue((double)item.Col1);
                            }

                            // F11 -> col2
                            ICell cellF = row.GetCell(5);
                            if (cellF != null && item.Col2.HasValue)
                            {
                                cellF.SetCellValue((double)item.Col2);
                            }

                            // G11 -> tangGiam1_2
                            ICell cellG = row.GetCell(6);
                            if (cellG != null && item.TangGiam1_2.HasValue)
                            {
                                cellG.SetCellValue((double)item.TangGiam1_2);
                            }

                            ICell cellH = row.GetCell(7);
                            if (cellH != null)
                            {
                                if (item.Col1.HasValue && item.Col2.HasValue && item.Col1.Value != 0)
                                {
                                    double rateOfIncreaseAndDecrease = (double)((item.Col2.Value - item.Col1.Value) / item.Col1.Value);
                                    cellH.SetCellValue(rateOfIncreaseAndDecrease);
                                }
                                else
                                {
                                    cellH.SetCellValue(0);
                                }
                            }

                        }

                        rowIndex++;
                    }

                    int rowIndex2 = 15;
                    foreach (var item in data.DLG.Dlg_TDGBL)
                    {
                        IRow row = sheet.GetRow(rowIndex2); // Chỉ lấy row, không cần CreateRow
                        if (row != null && !item.Code.Trim().Equals("701001", StringComparison.OrdinalIgnoreCase))
                        {
                            // B11 -> colA
                            ICell cellB = row.GetCell(1);
                            if (cellB != null)
                            {
                                cellB.SetCellValue(item.ColA);
                            }

                            // E11 -> col1
                            ICell cellE = row.GetCell(4);
                            if (cellE != null && item.Col3.HasValue)
                            {
                                cellE.SetCellValue((double)item.Col3);
                            }

                            // F11 -> col2
                            ICell cellF = row.GetCell(5);
                            if (cellF != null && item.Col4.HasValue)
                            {
                                cellF.SetCellValue((double)item.Col4);
                            }

                            // G11 -> tangGiam1_2
                            ICell cellG = row.GetCell(6);
                            if (cellG != null && item.TangGiam3_4.HasValue)
                            {
                                cellG.SetCellValue((double)item.TangGiam3_4);
                            }

                            ICell cellH = row.GetCell(7);
                            if (cellH != null)
                            {
                                if (item.Col3.HasValue && item.Col4.HasValue && item.Col3.Value != 0)
                                {
                                    double rateOfIncreaseAndDecrease = (double)((item.Col4.Value - item.Col3.Value) / item.Col3.Value);
                                    cellH.SetCellValue(rateOfIncreaseAndDecrease);
                                }
                                else
                                {
                                    cellH.SetCellValue(0);
                                }
                            }

                        }

                        rowIndex2++;
                    }

                    ISheet sheetCheck = workbook.GetSheetAt(0);
                    IRow rowCheck = sheetCheck.GetRow(4);
                    ICell cellCheck = rowCheck?.GetCell(0);
                    Console.WriteLine($"Giá trị trước khi lưu file: {cellCheck?.StringCellValue}");

                    // Ghi file
                    using (var fsOut = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(fsOut);
                        Console.WriteLine("Ghi file thành công");
                    }
                    workbook.Close();
                    return $"{folderName}/{fileName}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                throw;
            }
        }



        public void ExportExcel(ref MemoryStream outFileStream, string path, string headerId)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
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
                var startRowDB = 7;
                ISheet sheetDB = templateWorkbook.GetSheetAt(2);
                styleCellBold.CloneStyleFrom(sheetDB.GetRow(1).Cells[0].CellStyle);


                for (var i = 0; i < data.Result.DB.Count(); i++)
                {
                    var dataRow = data.Result.DB[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetDB, startRowDB++, 43);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);
                    rowCur.Cells[1].SetCellValue(dataRow.ColB);
                    rowCur.Cells[2].SetCellValue(dataRow.Col1);
                    rowCur.Cells[3].CellStyle = styleCellNumber;
                    rowCur.Cells[3].SetCellValue(dataRow.Col2 == 0 ? 0 : Convert.ToDouble(dataRow.Col2));

                    var iLG = 0;
                    for (var lg = iLG; lg < dataRow.LG.Count(); lg++)
                    {
                        rowCur.Cells[4 + lg].CellStyle = styleCellNumber;
                        rowCur.Cells[4 + lg].SetCellValue(dataRow.LG[lg] == 0 ? 0 : Convert.ToDouble(dataRow.LG[lg]));
                    }

                    rowCur.Cells[9].CellStyle = styleCellNumber;
                    rowCur.Cells[9].SetCellValue(dataRow.Col3 == 0 ? 0 : Convert.ToDouble(dataRow.Col3));

                    rowCur.Cells[10].CellStyle = styleCellNumber;
                    rowCur.Cells[10].SetCellValue(dataRow.Col4 == 0 ? 0 : Convert.ToDouble(dataRow.Col4));

                    rowCur.Cells[11].CellStyle = styleCellNumber;
                    rowCur.Cells[11].SetCellValue(dataRow.Col5 == 0 ? 0 : Convert.ToDouble(dataRow.Col5));

                    rowCur.Cells[12].CellStyle = styleCellNumber;
                    rowCur.Cells[12].SetCellValue(dataRow.Col6 == 0 ? 0 : Convert.ToDouble(dataRow.Col6));

                    rowCur.Cells[13].CellStyle = styleCellNumber;
                    rowCur.Cells[13].SetCellValue(dataRow.Col7 == 0 ? 0 : Convert.ToDouble(dataRow.Col7));

                    rowCur.Cells[14].CellStyle = styleCellNumber;
                    rowCur.Cells[14].SetCellValue(dataRow.Col8 == 0 ? 0 : Convert.ToDouble(dataRow.Col8));

                    rowCur.Cells[15].CellStyle = styleCellNumber;
                    rowCur.Cells[15].SetCellValue(dataRow.Col9 == 0 ? 0 : Convert.ToDouble(dataRow.Col9));

                    rowCur.Cells[16].CellStyle = styleCellNumber;
                    rowCur.Cells[16].SetCellValue(dataRow.Col10 == 0 ? 0 : Convert.ToDouble(dataRow.Col10));

                    var iGG = 0;
                    for (var gg = 0; gg < dataRow.GG.Count(); gg++)
                    {
                        rowCur.Cells[17 + iGG].CellStyle = styleCellNumber;
                        rowCur.Cells[17 + iGG].SetCellValue(dataRow.GG[gg].VAT == 0 ? 0 : Convert.ToDouble(dataRow.GG[gg].VAT));
                        rowCur.Cells[18 + iGG].CellStyle = styleCellNumber;
                        rowCur.Cells[18 + iGG].SetCellValue(dataRow.GG[gg].NonVAT == 0 ? 0 : Convert.ToDouble(dataRow.GG[gg].NonVAT));
                        iGG += 2;
                    }

                    //rowCur.Cells[27].CellStyle = 0;

                    var iLN = 0;
                    for (var ln = iLN; ln < dataRow.LG.Count(); ln++)
                    {
                        rowCur.Cells[28 + ln].CellStyle = styleCellNumber;
                        rowCur.Cells[28 + ln].SetCellValue(dataRow.LN[ln] == 0 ? 0 : Convert.ToDouble(dataRow.LN[ln]));
                    }

                    var iBV = 0;
                    for (var gg = 0; gg < dataRow.BVMT.Count(); gg++)
                    {
                        rowCur.Cells[33 + iBV].CellStyle = styleCellNumber;
                        rowCur.Cells[33 + iBV].SetCellValue(dataRow.BVMT[gg].NonVAT == 0 ? 0 : Convert.ToDouble(dataRow.BVMT[gg].NonVAT));
                        rowCur.Cells[34 + iBV].CellStyle = styleCellNumber;
                        rowCur.Cells[34 + iBV].SetCellValue(dataRow.BVMT[gg].VAT == 0 ? 0 : Convert.ToDouble(dataRow.BVMT[gg].VAT));
                        iBV += 2;
                    }

                    for (var j = 0; j < 43; j++)
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

                #region Export FOB

                var startRowFOB = 7;
                ISheet sheetFOB = templateWorkbook.GetSheetAt(3);
                styleCellBold.CloneStyleFrom(sheetFOB.GetRow(1).Cells[0].CellStyle);


                for (var i = 0; i < data.Result.FOB.Count(); i++)
                {
                    var dataRow = data.Result.FOB[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetFOB, startRowFOB++, 40);

                    rowCur.Cells[0].SetCellValue(dataRow.ColA);
                    rowCur.Cells[1].SetCellValue(dataRow.ColB);

                    var iLG = 0;
                    for (var lg = iLG; lg < dataRow.LG.Count(); lg++)
                    {
                        rowCur.Cells[2 + lg].CellStyle = styleCellNumber;
                        rowCur.Cells[2 + lg].SetCellValue(dataRow.LG[lg] == 0 ? 0 : Convert.ToDouble(dataRow.LG[lg]));
                    }

                    rowCur.Cells[7].CellStyle = styleCellNumber;
                    rowCur.Cells[7].SetCellValue(dataRow.Col1 == 0 ? 0 : Convert.ToDouble(dataRow.Col1));

                    rowCur.Cells[8].CellStyle = styleCellNumber;
                    rowCur.Cells[8].SetCellValue(dataRow.Col2 == 0 ? 0 : Convert.ToDouble(dataRow.Col2));

                    rowCur.Cells[9].CellStyle = styleCellNumber;
                    rowCur.Cells[9].SetCellValue(dataRow.Col3 == 0 ? 0 : Convert.ToDouble(dataRow.Col3));

                    rowCur.Cells[10].CellStyle = styleCellNumber;
                    rowCur.Cells[10].SetCellValue(dataRow.Col4 == 0 ? 0 : Convert.ToDouble(dataRow.Col4));

                    rowCur.Cells[11].CellStyle = styleCellNumber;
                    rowCur.Cells[11].SetCellValue(dataRow.Col5 == 0 ? 0 : Convert.ToDouble(dataRow.Col5));

                    rowCur.Cells[12].CellStyle = styleCellNumber;
                    rowCur.Cells[12].SetCellValue(dataRow.Col6 == 0 ? 0 : Convert.ToDouble(dataRow.Col6));

                    rowCur.Cells[13].CellStyle = styleCellNumber;
                    rowCur.Cells[13].SetCellValue(dataRow.Col7 == 0 ? 0 : Convert.ToDouble(dataRow.Col7));

                    var iGG = 0;
                    for (var gg = 0; gg < dataRow.GG.Count(); gg++)
                    {
                        rowCur.Cells[13 + iGG].CellStyle = styleCellNumber;
                        rowCur.Cells[13 + iGG].SetCellValue(dataRow.GG[gg].VAT == 0 ? 0 : Convert.ToDouble(dataRow.GG[gg].VAT));
                        rowCur.Cells[14 + iGG].CellStyle = styleCellNumber;
                        rowCur.Cells[14 + iGG].SetCellValue(dataRow.GG[gg].NonVAT == 0 ? 0 : Convert.ToDouble(dataRow.GG[gg].NonVAT));
                        iGG += 2;
                    }

                    rowCur.Cells[23].CellStyle = styleCellNumber;
                    rowCur.Cells[23].SetCellValue(dataRow.Col8 == 0 ? 0 : Convert.ToDouble(dataRow.Col8));

                    var iLN = 0;
                    for (var ln = iLN; ln < dataRow.LG.Count(); ln++)
                    {
                        rowCur.Cells[24 + ln].CellStyle = styleCellNumber;
                        rowCur.Cells[24 + ln].SetCellValue(dataRow.LN[ln] == 0 ? 0 : Convert.ToDouble(dataRow.LN[ln]));
                    }

                    var iBV = 0;
                    for (var gg = 0; gg < dataRow.BVMT.Count(); gg++)
                    {
                        rowCur.Cells[29 + iBV].CellStyle = styleCellNumber;
                        rowCur.Cells[29 + iBV].SetCellValue(dataRow.BVMT[gg].NonVAT == 0 ? 0 : Convert.ToDouble(dataRow.BVMT[gg].NonVAT));
                        rowCur.Cells[30 + iBV].CellStyle = styleCellNumber;
                        rowCur.Cells[30 + iBV].SetCellValue(dataRow.BVMT[gg].VAT == 0 ? 0 : Convert.ToDouble(dataRow.BVMT[gg].VAT));
                        iBV += 2;
                    }

                    for (var j = 0; j < 40; j++)
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

                #region Export PT09

                var startRowPT09 = 7;
                ISheet sheetPT09 = templateWorkbook.GetSheetAt(4);
                styleCellBold.CloneStyleFrom(sheetPT09.GetRow(1).Cells[0].CellStyle);


                for (var i = 0; i < data.Result.PT09.Count(); i++)
                {
                    var dataRow = data.Result.PT09[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetPT09, startRowPT09++, 39);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);
                    rowCur.Cells[1].SetCellValue(dataRow.ColB);

                    var iLG = 0;
                    for (var lg = iLG; lg < dataRow.LG.Count(); lg++)
                    {
                        rowCur.Cells[2 + lg].CellStyle = styleCellNumber;
                        rowCur.Cells[2 + lg].SetCellValue(dataRow.LG[lg] == 0 ? 0 : Convert.ToDouble(dataRow.LG[lg]));
                    }

                    rowCur.Cells[7].CellStyle = styleCellNumber;
                    rowCur.Cells[7].SetCellValue(dataRow.Col3 == 0 ? 0 : Convert.ToDouble(dataRow.Col3));

                    rowCur.Cells[8].CellStyle = styleCellNumber;
                    rowCur.Cells[8].SetCellValue(dataRow.Col4 == 0 ? 0 : Convert.ToDouble(dataRow.Col4));

                    rowCur.Cells[9].CellStyle = styleCellNumber;
                    rowCur.Cells[9].SetCellValue(dataRow.Col5 == 0 ? 0 : Convert.ToDouble(dataRow.Col5));

                    rowCur.Cells[10].CellStyle = styleCellNumber;
                    rowCur.Cells[10].SetCellValue(dataRow.Col6 == 0 ? 0 : Convert.ToDouble(dataRow.Col6));

                    rowCur.Cells[11].CellStyle = styleCellNumber;
                    rowCur.Cells[11].SetCellValue(dataRow.Col7 == 0 ? 0 : Convert.ToDouble(dataRow.Col7));

                    rowCur.Cells[12].CellStyle = styleCellNumber;
                    rowCur.Cells[12].SetCellValue(dataRow.Col8 == 0 ? 0 : Convert.ToDouble(dataRow.Col8));


                    var iGG = 0;
                    for (var gg = 0; gg < dataRow.GG.Count(); gg++)
                    {
                        rowCur.Cells[13 + iGG].CellStyle = styleCellNumber;
                        rowCur.Cells[13 + iGG].SetCellValue(dataRow.GG[gg].VAT == 0 ? 0 : Convert.ToDouble(dataRow.GG[gg].VAT));
                        rowCur.Cells[14 + iGG].CellStyle = styleCellNumber;
                        rowCur.Cells[14 + iGG].SetCellValue(dataRow.GG[gg].NonVAT == 0 ? 0 : Convert.ToDouble(dataRow.GG[gg].NonVAT));
                        iGG += 2;
                    }

                    rowCur.Cells[23].CellStyle = styleCellNumber;
                    rowCur.Cells[23].SetCellValue(dataRow.Col18 == 0 ? 0 : Convert.ToDouble(dataRow.Col18));

                    var iLN = 0;
                    for (var ln = iLN; ln < dataRow.LG.Count(); ln++)
                    {
                        rowCur.Cells[24 + ln].CellStyle = styleCellNumber;
                        rowCur.Cells[24 + ln].SetCellValue(dataRow.LN[ln] == 0 ? 0 : Convert.ToDouble(dataRow.LN[ln]));
                    }

                    var iBV = 0;
                    for (var gg = 0; gg < dataRow.BVMT.Count(); gg++)
                    {
                        rowCur.Cells[29 + iBV].CellStyle = styleCellNumber;
                        rowCur.Cells[29 + iBV].SetCellValue(dataRow.BVMT[gg].NonVAT == 0 ? 0 : Convert.ToDouble(dataRow.BVMT[gg].NonVAT));
                        rowCur.Cells[30 + iBV].CellStyle = styleCellNumber;
                        rowCur.Cells[30 + iBV].SetCellValue(dataRow.BVMT[gg].VAT == 0 ? 0 : Convert.ToDouble(dataRow.BVMT[gg].VAT));
                        iBV += 2;
                    }   

                    for (var j = 0; j < 38; j++)
                    {
                        rowCur.Cells[j].CellStyle.SetFont(font);
                        rowCur.Cells[j].CellStyle.BorderBottom = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderTop = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderLeft = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderRight = BorderStyle.Thin;
                    }
                }

                #endregion

                #region Export BB ĐO

                var startRowBBDO = 9;
                ISheet sheetBBDO = templateWorkbook.GetSheetAt(5);
                styleCellBold.CloneStyleFrom(sheetBBDO.GetRow(1).Cells[0].CellStyle);


                for (var i = 0; i < data.Result.BBDO.Count(); i++)
                {
                    var dataRow = data.Result.BBDO[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetBBDO, startRowBBDO++, 23);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);
                    rowCur.Cells[1].SetCellValue(dataRow.ColB);
                    rowCur.Cells[2].SetCellValue(dataRow.ColC);
                    rowCur.Cells[3].SetCellValue(dataRow.ColD);

                    rowCur.Cells[4].SetCellValue(dataRow.Col1);
                    rowCur.Cells[5].SetCellValue(dataRow.Col2);
                    rowCur.Cells[6].SetCellValue(dataRow.Col3);
                    rowCur.Cells[7].SetCellValue(dataRow.Col4);
                    rowCur.Cells[8].SetCellValue(dataRow.Col5);


                    rowCur.Cells[9].CellStyle = styleCellNumber;
                    rowCur.Cells[9].SetCellValue(dataRow.Col6 == 0 ? 0 : Convert.ToDouble(dataRow.Col6));

                    rowCur.Cells[10].CellStyle = styleCellNumber;
                    rowCur.Cells[10].SetCellValue(dataRow.Col7 == 0 ? 0 : Convert.ToDouble(dataRow.Col7));

                    rowCur.Cells[11].CellStyle = styleCellNumber;
                    rowCur.Cells[11].SetCellValue(dataRow.Col8 == 0 ? 0 : Convert.ToDouble(dataRow.Col8));

                    rowCur.Cells[12].CellStyle = styleCellNumber;
                    rowCur.Cells[12].SetCellValue(dataRow.Col9 == 0 ? 0 : Convert.ToDouble(dataRow.Col9));

                    rowCur.Cells[13].CellStyle = styleCellNumber;
                    rowCur.Cells[13].SetCellValue(dataRow.Col10 == 0 ? 0 : Convert.ToDouble(dataRow.Col10));

                    rowCur.Cells[14].CellStyle = styleCellNumber;
                    rowCur.Cells[14].SetCellValue(dataRow.Col11 == 0 ? 0 : Convert.ToDouble(dataRow.Col11));

                    rowCur.Cells[15].CellStyle = styleCellNumber;
                    rowCur.Cells[15].SetCellValue(dataRow.Col12 == 0 ? 0 : Convert.ToDouble(dataRow.Col12));

                    rowCur.Cells[16].CellStyle = styleCellNumber;
                    rowCur.Cells[16].SetCellValue(dataRow.Col13 == 0 ? 0 : Convert.ToDouble(dataRow.Col13));

                    rowCur.Cells[17].CellStyle = styleCellNumber;
                    rowCur.Cells[17].SetCellValue(dataRow.Col14 == 0 ? 0 : Convert.ToDouble(dataRow.Col14));

                    rowCur.Cells[18].CellStyle = styleCellNumber;
                    rowCur.Cells[18].SetCellValue(dataRow.Col15 == 0 ? 0 : Convert.ToDouble(dataRow.Col15));

                    rowCur.Cells[19].CellStyle = styleCellNumber;
                    rowCur.Cells[19].SetCellValue(dataRow.Col16 == 0 ? 0 : Convert.ToDouble(dataRow.Col16));

                    rowCur.Cells[20].CellStyle = styleCellNumber;
                    rowCur.Cells[20].SetCellValue(dataRow.Col17 == 0 ? 0 : Convert.ToDouble(dataRow.Col17));

                    rowCur.Cells[21].CellStyle = styleCellNumber;
                    rowCur.Cells[21].SetCellValue(dataRow.Col18 == 0 ? 0 : Convert.ToDouble(dataRow.Col18));

                    rowCur.Cells[22].CellStyle = styleCellNumber;
                    rowCur.Cells[22].SetCellValue(dataRow.Col19 == 0 ? 0 : Convert.ToDouble(dataRow.Col19));

                    for (var j = 0; j < 23; j++)
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

                #region Export BB FO

                var startRowBBFO = 11;
                ISheet sheetBBFO = templateWorkbook.GetSheetAt(6);
                styleCellBold.CloneStyleFrom(sheetBBFO.GetRow(1).Cells[0].CellStyle);


                for (var i = 0; i < data.Result.BBFO.Count(); i++)
                {
                    var dataRow = data.Result.BBFO[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetBBFO, startRowBBFO++, 14);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);
                    rowCur.Cells[1].SetCellValue(dataRow.ColB);
                    rowCur.Cells[2].SetCellValue(dataRow.ColC);


                    rowCur.Cells[4].CellStyle = styleCellNumber;
                    rowCur.Cells[4].SetCellValue(dataRow.Col1 == 0 ? 0 : Convert.ToDouble(dataRow.Col1));

                    rowCur.Cells[5].CellStyle = styleCellNumber;
                    rowCur.Cells[5].SetCellValue(dataRow.Col2 == 0 ? 0 : Convert.ToDouble(dataRow.Col2));

                    rowCur.Cells[6].CellStyle = styleCellNumber;
                    rowCur.Cells[6].SetCellValue(dataRow.Col3 == 0 ? 0 : Convert.ToDouble(dataRow.Col3));

                    rowCur.Cells[7].CellStyle = styleCellNumber;
                    rowCur.Cells[7].SetCellValue(dataRow.Col4 == 0 ? 0 : Convert.ToDouble(dataRow.Col4));

                    rowCur.Cells[8].CellStyle = styleCellNumber;
                    rowCur.Cells[8].SetCellValue(dataRow.Col5 == 0 ? 0 : Convert.ToDouble(dataRow.Col5));

                    rowCur.Cells[9].CellStyle = styleCellNumber;
                    rowCur.Cells[9].SetCellValue(dataRow.Col6 == 0 ? 0 : Convert.ToDouble(dataRow.Col6));

                    rowCur.Cells[10].CellStyle = styleCellNumber;
                    rowCur.Cells[10].SetCellValue(dataRow.Col7 == 0 ? 0 : Convert.ToDouble(dataRow.Col7));

                    rowCur.Cells[11].CellStyle = styleCellNumber;
                    rowCur.Cells[11].SetCellValue(dataRow.Col8 == 0 ? 0 : Convert.ToDouble(dataRow.Col8));

                    rowCur.Cells[12].CellStyle = styleCellNumber;
                    rowCur.Cells[12].SetCellValue(dataRow.Col9 == 0 ? 0 : Convert.ToDouble(dataRow.Col9));

                    rowCur.Cells[13].CellStyle = styleCellNumber;
                    rowCur.Cells[13].SetCellValue(dataRow.Col10 == 0 ? 0 : Convert.ToDouble(dataRow.Col10));
                    for (var j = 0; j < 14; j++)
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

                #region Export PL1

                var startRowPL1 = 8;
                ISheet sheetPL1 = templateWorkbook.GetSheetAt(7);
                styleCellBold.CloneStyleFrom(sheetPL1.GetRow(1).Cells[0].CellStyle);


                for (var i = 0; i < data.Result.PL1.Count(); i++)
                {
                    var dataRow = data.Result.PL1[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetPL1, startRowPL1++, 7);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);
                    rowCur.Cells[1].SetCellValue(dataRow.ColB);

                    var iGG = 0;
                    for (var gg = 0; gg < dataRow.GG.Count(); gg++)
                    {
                        rowCur.Cells[2 + iGG].CellStyle = styleCellNumber;
                        rowCur.Cells[2 + iGG].SetCellValue(dataRow.GG[gg] == 0 ? 0 : Convert.ToDouble(dataRow.GG[gg]));
                        iGG += 1;
                    }
                    for (var j = 0; j < 7; j++)
                    {
                        rowCur.Cells[j].CellStyle.SetFont(font);
                        rowCur.Cells[j].CellStyle.BorderBottom = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderTop = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderLeft = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderRight = BorderStyle.Thin;
                    }
                }

                #endregion

                #region Export PL2

                var startRowPL2 = 7;
                ISheet sheetPL2 = templateWorkbook.GetSheetAt(8);
                styleCellBold.CloneStyleFrom(sheetPL2.GetRow(1).Cells[0].CellStyle);


                for (var i = 0; i < data.Result.PL2.Count(); i++)
                {
                    var dataRow = data.Result.PL2[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetPL2, startRowPL2++, 7);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);
                    rowCur.Cells[1].SetCellValue(dataRow.ColB);

                    var iGG = 0;
                    for (var gg = 0; gg < dataRow.GG.Count(); gg++)
                    {
                        rowCur.Cells[2 + iGG].CellStyle = styleCellNumber;
                        rowCur.Cells[2 + iGG].SetCellValue(dataRow.GG[gg] == 0 ? 0 : Convert.ToDouble(dataRow.GG[gg]));
                        iGG += 1;
                    }
                    for (var j = 0; j < 7; j++)
                    {
                        rowCur.Cells[j].CellStyle.SetFont(font);
                        rowCur.Cells[j].CellStyle.BorderBottom = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderTop = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderLeft = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderRight = BorderStyle.Thin;
                    }
                }

                #endregion

                #region Export PL3

                var startRowPL3 = 7;
                ISheet sheetPL3 = templateWorkbook.GetSheetAt(9);
                styleCellBold.CloneStyleFrom(sheetPL3.GetRow(1).Cells[0].CellStyle);


                for (var i = 0; i < data.Result.PL3.Count(); i++)
                {
                    var dataRow = data.Result.PL3[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetPL3, startRowPL3++, 7);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);
                    rowCur.Cells[1].SetCellValue(dataRow.ColB);

                    var iGG = 0;
                    for (var gg = 0; gg < dataRow.GG.Count(); gg++)
                    {
                        rowCur.Cells[2 + iGG].CellStyle = styleCellNumber;
                        rowCur.Cells[2 + iGG].SetCellValue(dataRow.GG[gg] == 0 ? 0 : Convert.ToDouble(dataRow.GG[gg]));
                        iGG += 1;
                    }
                    for (var j = 0; j < 7; j++)
                    {
                        rowCur.Cells[j].CellStyle.SetFont(font);
                        rowCur.Cells[j].CellStyle.BorderBottom = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderTop = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderLeft = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderRight = BorderStyle.Thin;
                    }

                }

                #endregion

                #region Export PL4

                var startRowPL4 = 8;
                ISheet sheetPL4 = templateWorkbook.GetSheetAt(10);
                styleCellBold.CloneStyleFrom(sheetPL4.GetRow(1).Cells[0].CellStyle);


                for (var i = 0; i < data.Result.PL4.Count(); i++)
                {
                    var dataRow = data.Result.PL4[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetPL4, startRowPL4++, 7);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);
                    rowCur.Cells[1].SetCellValue(dataRow.ColB);

                    var iGG = 0;
                    for (var gg = 0; gg < dataRow.GG.Count(); gg++)
                    {
                        rowCur.Cells[2 + iGG].CellStyle = styleCellNumber;
                        rowCur.Cells[2 + iGG].SetCellValue(dataRow.GG[gg] == 0 ? 0 : Convert.ToDouble(dataRow.GG[gg]));
                        iGG += 1;
                    }
                    for (var j = 0; j < 7; j++)
                    {
                        rowCur.Cells[j].CellStyle.SetFont(font);
                        rowCur.Cells[j].CellStyle.BorderBottom = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderTop = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderLeft = BorderStyle.Thin;
                        rowCur.Cells[j].CellStyle.BorderRight = BorderStyle.Thin;
                    }
                }

                #endregion

                #region Export VK11-PT

                var startRowVK11PT = 2;
                ISheet sheetVK11PT = templateWorkbook.GetSheetAt(11);
                styleCellBold.CloneStyleFrom(sheetVK11PT.GetRow(1).Cells[0].CellStyle);


                for (var i = 0; i < data.Result.VK11PT.Count(); i++)
                {
                    var dataRow = data.Result.VK11PT[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetVK11PT, startRowVK11PT++, 20);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);
                    rowCur.Cells[1].SetCellValue(dataRow.ColB);
                    rowCur.Cells[2].SetCellValue(dataRow.Col1);

                    rowCur.Cells[3].CellStyle = styleCellNumber;
                    rowCur.Cells[3].SetCellValue(dataRow.Col2 == 0 ? 0 : Convert.ToDouble(dataRow.Col2));

                    rowCur.Cells[4].CellStyle = styleCellNumber;
                    rowCur.Cells[4].SetCellValue(dataRow.Col3 == 0 ? 0 : Convert.ToDouble(dataRow.Col3));

                    rowCur.Cells[5].SetCellValue(dataRow.Col4);
                    rowCur.Cells[6].SetCellValue(dataRow.Col5);
                    rowCur.Cells[7].SetCellValue(dataRow.Col6);
                    rowCur.Cells[8].SetCellValue(dataRow.Col7);
                    rowCur.Cells[9].SetCellValue(dataRow.Col8);

                    rowCur.Cells[10].CellStyle = styleCellNumber;
                    rowCur.Cells[10].SetCellValue(dataRow.Col9 == 0 ? 0 : Convert.ToDouble(dataRow.Col9));
                    
                    rowCur.Cells[11].SetCellValue(dataRow.Col10);

                    rowCur.Cells[12].CellStyle = styleCellNumber;
                    rowCur.Cells[12].SetCellValue(dataRow.Col11 == 0 ? 0 : Convert.ToDouble(dataRow.Col11));
                    

                    rowCur.Cells[13].SetCellValue(dataRow.Col12);
                    rowCur.Cells[14].SetCellValue(dataRow.Col13);
                    rowCur.Cells[15].SetCellValue(dataRow.Col14);
                    rowCur.Cells[16].SetCellValue(dataRow.Col15);
                    rowCur.Cells[17].SetCellValue(dataRow.Col16);
                    rowCur.Cells[18].SetCellValue(dataRow.Col17);
                    rowCur.Cells[19].SetCellValue(dataRow.Col18);


                    for (var j = 0; j < 20; j++)
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

                #region Export VK11-DB

                var startRowVK11DB = 2;
                ISheet sheetVK11DB = templateWorkbook.GetSheetAt(12);
                styleCellBold.CloneStyleFrom(sheetVK11DB.GetRow(1).Cells[0].CellStyle);

                for (var i = 0; i < data.Result.VK11DB.Count(); i++)
                {
                    var dataRow = data.Result.VK11DB[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetVK11DB, startRowVK11DB++, 21);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);
                    rowCur.Cells[1].SetCellValue(dataRow.ColB);
                    rowCur.Cells[2].SetCellValue(dataRow.ColC);
                    rowCur.Cells[3].SetCellValue(dataRow.Col1);

                    rowCur.Cells[4].CellStyle = styleCellNumber;
                    rowCur.Cells[4].SetCellValue(dataRow.Col2 == 0 ? 0 : Convert.ToDouble(dataRow.Col2));

                    rowCur.Cells[5].CellStyle = styleCellNumber;
                    rowCur.Cells[5].SetCellValue(dataRow.Col3 == 0 ? 0 : Convert.ToDouble(dataRow.Col3));

                    rowCur.Cells[6].SetCellValue(dataRow.Col4);
                    rowCur.Cells[7].SetCellValue(dataRow.Col5);
                    rowCur.Cells[8].SetCellValue(dataRow.Col6);
                    rowCur.Cells[9].SetCellValue(dataRow.Col7);
                    rowCur.Cells[10].SetCellValue(dataRow.Col8);

                    rowCur.Cells[11].CellStyle = styleCellNumber;
                    rowCur.Cells[11].SetCellValue(dataRow.Col9 == 0 ? 0 : Convert.ToDouble(dataRow.Col9));

                    rowCur.Cells[12].SetCellValue(dataRow.Col10);

                    rowCur.Cells[13].CellStyle = styleCellNumber;
                    rowCur.Cells[13].SetCellValue(dataRow.Col11 == 0 ? 0 : Convert.ToDouble(dataRow.Col11));

                    rowCur.Cells[14].SetCellValue(dataRow.Col12);
                    rowCur.Cells[15].SetCellValue(dataRow.Col13);
                    rowCur.Cells[16].SetCellValue(dataRow.Col14);
                    rowCur.Cells[17].SetCellValue(dataRow.Col15);
                    rowCur.Cells[18].SetCellValue(dataRow.Col16);
                    rowCur.Cells[19].SetCellValue(dataRow.Col17);
                    rowCur.Cells[20].SetCellValue(dataRow.Col18);

                    for (var j = 0; j < 21; j++)
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

                #region Export VK11-FOB

                var startRowVK11FOB = 3;
                ISheet sheetVK11FOB = templateWorkbook.GetSheetAt(13);

                for (var i = 0; i < data.Result.VK11FOB.Count(); i++)
                {
                    var dataRow = data.Result.VK11FOB[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetVK11FOB, startRowVK11FOB++, 21);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);
                    //rowCur.Cells[1].SetCellValue(dataRow.ColB);
                    rowCur.Cells[1].SetCellValue(dataRow.ColC == null ? dataRow.ColB : dataRow.ColC);
                    //rowCur.Cells[1].SetCellValue(dataRow.ColC);

                    rowCur.Cells[2].SetCellValue(dataRow.Col1);

                    rowCur.Cells[3].CellStyle = styleCellNumber;
                    rowCur.Cells[3].SetCellValue(dataRow.Col2 == 0 ? 0 : Convert.ToDouble(dataRow.Col2));

                    rowCur.Cells[4].CellStyle = styleCellNumber;
                    rowCur.Cells[4].SetCellValue(dataRow.Col3 == 0 ? 0 : Convert.ToDouble(dataRow.Col3));

                    rowCur.Cells[5].SetCellValue(dataRow.Col4);
                    rowCur.Cells[6].SetCellValue(dataRow.Col5);
                    rowCur.Cells[7].SetCellValue(dataRow.Col6);
                    rowCur.Cells[8].SetCellValue(dataRow.Col7);
                    rowCur.Cells[9].SetCellValue(dataRow.Col8);

                    rowCur.Cells[10].CellStyle = styleCellNumber;
                    rowCur.Cells[10].SetCellValue(dataRow.Col9 == 0 ? 0 : Convert.ToDouble(dataRow.Col9));

                    rowCur.Cells[11].SetCellValue(dataRow.Col10);

                    rowCur.Cells[12].CellStyle = styleCellNumber;
                    rowCur.Cells[12].SetCellValue(dataRow.Col11 == 0 ? 0 : Convert.ToDouble(dataRow.Col11));

                    rowCur.Cells[13].SetCellValue(dataRow.Col12);
                    rowCur.Cells[14].SetCellValue(dataRow.Col13);
                    rowCur.Cells[15].SetCellValue(dataRow.Col14);
                    rowCur.Cells[16].SetCellValue(dataRow.Col15);
                    rowCur.Cells[17].SetCellValue(dataRow.Col16);
                    rowCur.Cells[18].SetCellValue(dataRow.Col17);
                    rowCur.Cells[19].SetCellValue(dataRow.Col18);

                    for (var j = 0; j < 20; j++)
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

                #region Export VK11-TNPP

                var startRowVK11TNPP = 3;
                ISheet sheetVK11TNPP = templateWorkbook.GetSheetAt(14);

                for (var i = 0; i < data.Result.VK11TNPP.Count(); i++)
                {
                    var dataRow = data.Result.VK11TNPP[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetVK11TNPP, startRowVK11TNPP++, 20);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);

                    //rowCur.Cells[1].SetCellValue(dataRow.ColB);
                    rowCur.Cells[1].SetCellValue(dataRow.ColC == null ? dataRow.ColB : dataRow.ColC);

                    rowCur.Cells[2].SetCellValue(dataRow.Col1);

                    rowCur.Cells[3].CellStyle = styleCellNumber;
                    rowCur.Cells[3].SetCellValue(dataRow.Col2 == 0 ? 0 : Convert.ToDouble(dataRow.Col2));

                    rowCur.Cells[4].CellStyle = styleCellNumber;
                    rowCur.Cells[4].SetCellValue(dataRow.Col3 == 0 ? 0 : Convert.ToDouble(dataRow.Col3));

                    rowCur.Cells[5].SetCellValue(dataRow.Col4);
                    rowCur.Cells[6].SetCellValue(dataRow.Col5);
                    rowCur.Cells[7].SetCellValue(dataRow.Col6);
                    rowCur.Cells[8].SetCellValue(dataRow.Col7);
                    rowCur.Cells[9].SetCellValue(dataRow.Col8);

                    rowCur.Cells[10].CellStyle = styleCellNumber;
                    rowCur.Cells[10].SetCellValue(dataRow.Col9 == 0 ? 0 : Convert.ToDouble(dataRow.Col9));

                    rowCur.Cells[11].SetCellValue(dataRow.Col10);

                    rowCur.Cells[12].CellStyle = styleCellNumber;
                    rowCur.Cells[12].SetCellValue(dataRow.Col11 == 0 ? 0 : Convert.ToDouble(dataRow.Col11));

                    rowCur.Cells[13].SetCellValue(dataRow.Col12);
                    rowCur.Cells[14].SetCellValue(dataRow.Col13);
                    rowCur.Cells[15].SetCellValue(dataRow.Col14);
                    rowCur.Cells[16].SetCellValue(dataRow.Col15);
                    rowCur.Cells[17].SetCellValue(dataRow.Col16);
                    rowCur.Cells[18].SetCellValue(dataRow.Col17);
                    rowCur.Cells[19].SetCellValue(dataRow.Col18);

                    for (var j = 0; j < 20; j++)
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

                #region Export VK11-BB

                var startRowVK11BB = 3;
                ISheet sheetVK11BB = templateWorkbook.GetSheetAt(15);

                for (var i = 0; i < data.Result.VK11BB.Count(); i++)
                {
                    var dataRow = data.Result.VK11BB[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetVK11BB, startRowVK11BB++, 18);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);

                    rowCur.Cells[1].SetCellValue(dataRow.ColB);
                    rowCur.Cells[2].SetCellValue(dataRow.ColC);

                    rowCur.Cells[3].SetCellValue(dataRow.Col1);
                    rowCur.Cells[4].SetCellValue(dataRow.Col2);
                    rowCur.Cells[5].SetCellValue(dataRow.Col3);
                    rowCur.Cells[6].SetCellValue(dataRow.Col4);
                    rowCur.Cells[7].SetCellValue(dataRow.Col5);

                    rowCur.Cells[8].CellStyle = styleCellNumber;
                    rowCur.Cells[8].SetCellValue(dataRow.Col6 == 0 ? 0 : Convert.ToDouble(dataRow.Col6));

                    rowCur.Cells[9].SetCellValue(dataRow.Col7);
                    rowCur.Cells[10].SetCellValue(dataRow.Col8);
                    rowCur.Cells[11].SetCellValue(dataRow.Col9);
                    rowCur.Cells[12].SetCellValue(dataRow.Col10);
                    rowCur.Cells[13].SetCellValue(dataRow.Col11);

                    rowCur.Cells[14].SetCellValue(dataRow.Col12);
                    rowCur.Cells[15].SetCellValue(dataRow.Col13);
                    rowCur.Cells[16].SetCellValue(dataRow.Col14);
                    rowCur.Cells[17].SetCellValue(dataRow.Col15);

                    for (var j = 0; j < 18; j++)
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

                #region Export TongHop

                var startRowTH = 3;
                ISheet sheetTH = templateWorkbook.GetSheetAt(17);

                for (var i = 0; i < data.Result.Summary.Count(); i++)
                {
                    var dataRow = data.Result.Summary[i];
                    IRow rowCur = ReportUtilities.CreateRow(ref sheetTH, startRowTH++, 21);
                    rowCur.Cells[0].SetCellValue(dataRow.ColA);

                    rowCur.Cells[1].SetCellValue(dataRow.ColB);
                    rowCur.Cells[2].SetCellValue(dataRow.ColC);
                    rowCur.Cells[3].SetCellValue(dataRow.ColD);

                    rowCur.Cells[6].SetCellValue(dataRow.Col1);
                    rowCur.Cells[7].SetCellValue(dataRow.Col2);
                    rowCur.Cells[8].SetCellValue(dataRow.Col3);
                    rowCur.Cells[9].SetCellValue(dataRow.Col4);
                    rowCur.Cells[10].SetCellValue(dataRow.Col5);

                    rowCur.Cells[11].CellStyle = styleCellNumber;
                    rowCur.Cells[11].SetCellValue(dataRow.Col6 == 0 ? 0 : Convert.ToDouble(dataRow.Col6));

                    rowCur.Cells[12].SetCellValue(dataRow.Col7);
                    rowCur.Cells[13].SetCellValue(dataRow.Col8);
                    rowCur.Cells[14].SetCellValue(dataRow.Col9);
                    rowCur.Cells[15].SetCellValue(dataRow.Col10);
                    rowCur.Cells[16].SetCellValue(dataRow.Col11);

                    rowCur.Cells[17].SetCellValue(dataRow.Col12);
                    rowCur.Cells[18].SetCellValue(dataRow.Col13);
                    rowCur.Cells[19].SetCellValue(dataRow.Col14);
                    rowCur.Cells[20].SetCellValue(dataRow.Col15);

                    for (var j = 0; j < 21; j++)
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

        public async Task<string> SaveFileHistory(MemoryStream outFileStream, string headerId)
        {
            byte[] data = outFileStream.ToArray();
            var path = "";
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                IFormFile file = ConvertMemoryStreamToIFormFile(memoryStream, "example.txt");
                var folderName = Path.Combine($"Upload/{DateTime.Now.Year}/{DateTime.Now.Month}");
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = $"{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}_CoSoTinhMucGiamGia.xlsx";
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    path = $"Upload/{DateTime.Now.Year}/{DateTime.Now.Month}/{fileName}";
                    _dbContext.TblBuHistoryDownload.Add(new TblBuHistoryDownload
                    {
                        Code = Guid.NewGuid().ToString(),
                        HeaderCode = headerId,
                        Name = fileName,
                        Type = "xlsx",
                        Path = path
                    });
                    await _dbContext.SaveChangesAsync();
                }
            }
            return path;
        }
        
        public static IFormFile ConvertMemoryStreamToIFormFile(MemoryStream memoryStream, string fileName)
        {
            memoryStream.Position = 0; // Reset the stream position to the beginning
            IFormFile formFile = new FormFile(memoryStream, 0, memoryStream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream"
            };
            return formFile;

        }

        public async Task<string> GenarateWordTrinhKy(string headerId, string nameTemp)
        {
            #region Tạo 1 file word mới từ file template
            var filePathTemplate = Directory.GetCurrentDirectory() + $"/Template/TempTrinhKy/{nameTemp}.docx";
            var folderName = Path.Combine($"Upload/{DateTime.Now.Year}/{DateTime.Now.Month}");
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fileName = $"{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}_{nameTemp}.docx";
            var fullPath = Path.Combine(pathToSave, fileName);
            File.Copy(filePathTemplate, fullPath, true);
            #endregion

            #region Lấy các text element
            List<string> lstTextElement = new List<string>();
            WordDocumentService wordDocumentService = new WordDocumentService();
            using (WordprocessingDocument doc = WordprocessingDocument.Open(fullPath, true))
            {
                lstTextElement = wordDocumentService.FindTextElement(doc);
                lstTextElement = lstTextElement.Distinct().ToList();
            }
            #endregion

            #region Fill dữ liệu
            var data = await GetResult(headerId);
            var header = await _dbContext.TblBuCalculateResultList.FindAsync(headerId);
            var model = await GetDataInput(headerId);
            var goods = await _dbContext.TblMdGoods.ToListAsync();
            var NguoiKyTen = await _dbContext.TblInNguoiKyTen.FirstOrDefaultAsync(x => x.HeaderCode == headerId);  //
            var f_date = $"{header.FDate.Day} tháng {header.FDate.Month} năm {header.FDate.Year}";
            var date = $"{header.FDate.Day}/{header.FDate.Month}/{header.FDate.Year}";
            var f_date_hour = $"kể từ {header.FDate.Hour} giờ {header.FDate.Minute} ngày {header.FDate.Day} tháng {header.FDate.Month} năm {header.FDate.Year}";
            TableCell CreateCell(string text, bool isBold = true, int fontSize = 26, bool isCenter = true, string width = "2400", int? gridSpan = null, int v = 0)
            {
                Run run = new Run(new Text(text));

                RunProperties runProperties = new RunProperties(
                    new RunFonts()
                    {
                        Ascii = "Times New Roman",
                        HighAnsi = "Times New Roman",
                        ComplexScript = "Times New Roman"
                    },
                    new FontSize() { Val = new StringValue(fontSize.ToString()) }
                );

                if (isBold)
                {
                    runProperties.Append(new Bold());
                }

                run.RunProperties = runProperties;

                Paragraph paragraph = new Paragraph(run);

                if (isCenter)
                {
                    paragraph.ParagraphProperties = new ParagraphProperties(
                        new Justification() { Val = JustificationValues.Center }
                    );
                }

                TableCellProperties cellProperties = new TableCellProperties(
                    new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
                );

                if (!string.IsNullOrEmpty(width))
                {
                    cellProperties.Append(new TableCellWidth() { Width = width, Type = TableWidthUnitValues.Dxa });
                }

                // Nếu có gridSpan, áp dụng thuộc tính gộp ô
                if (gridSpan.HasValue && gridSpan > 1)
                {
                    cellProperties.Append(new GridSpan() { Val = gridSpan });
                }

                TableCell cell = new TableCell();
                cell.Append(cellProperties);
                cell.Append(paragraph);

                return cell;
            }

            #region fill dữ liệu file công điện kiểm kê giá bán lẻ
            if (nameTemp == "CongDienKKGiaBanLe")
            {
                var sortedHS2 = model.HS2.OrderBy(x => int.Parse(x.GoodsCode)).ToList();
                var dlg1 = data.DLG.Dlg_1;
                using (WordprocessingDocument doc = WordprocessingDocument.Open(fullPath, true))
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    DocumentFormat.OpenXml.Wordprocessing.Body body = mainPart.Document.Body;

                    foreach (var t in lstTextElement)
                    {
                        switch (t)
                        {
                            case "##F_DATE@@":
                                var text = $"ngày {header.FDate.Day} tháng {header.FDate.Month} năm {header.FDate.Year}";
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, text);
                                break;
                            case "##QUYET_DINH_SO@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.QuyetDinhSo);
                                break;
                            case "##DAI_DIEN@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.DaiDien);
                                break;
                            case "##NGUOI_DAI_DIEN@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.NguoiDaiDien);
                                break;
                            case "##TABLE_TT@@":
                                Paragraph paragraph = body.Descendants<Paragraph>()
                                               .FirstOrDefault(p => p.InnerText.Contains("##TABLE_TT@@"));
                                if (paragraph != null)
                                {
                                    Table table = new Table();
                                    DocumentFormat.OpenXml.Wordprocessing.TableProperties tblProperties = new DocumentFormat.OpenXml.Wordprocessing.TableProperties(
                                       new TableCellMarginDefault(
                                           new LeftMargin() { Width = "115" },
                                           new RightMargin() { Width = "115" },
                                           new TopMargin() { Width = "50" },
                                           new BottomMargin() { Width = "50" }
                                           
                                       )
                                   );
                                    table.AppendChild(tblProperties);

                                    #region Gendata table
                                    var o = 1;
                                    var goodsList = goods.Where(x => x.IsActive == true)
                                                         .OrderByDescending(x => x.ThueBvmt)
                                                         .ToList();
                                    foreach (var i in goodsList)
                                    {
                                        var HS2Item = model.HS2.FirstOrDefault(x => x.GoodsCode == i.Code);
                                        if (HS2Item != null)
                                        {
                                            TableRow row = new TableRow();
                                            row.Append(CreateCell("+ " + i.Name, true, 26, false, "3500"));
                                            row.Append(CreateCell(":", false, 26, true, "1"));
                                            row.Append(CreateCell(HS2Item.Gny.ToString("N0"), true, 26, false, "2400"));
                                            row.Append(CreateCell("đ/lít thực tế", false, 26, false, "2400"));                                           
                                            table.Append(row);
                                            o++;
                                        }
                                    }
                                    #endregion
                                    paragraph.Parent.InsertAfter(table, paragraph);
                                    paragraph.Remove();
                                }
                                break;
                            case "##TABLE_VCL@@":
                                Paragraph paragraph2 = body.Descendants<Paragraph>()
                                           .FirstOrDefault(p => p.InnerText.Contains("##TABLE_VCL@@"));
                                if (paragraph2 != null)
                                {
                                    Table table = new Table();
                                    DocumentFormat.OpenXml.Wordprocessing.TableProperties tblProperties = new DocumentFormat.OpenXml.Wordprocessing.TableProperties(
                                       new TableCellMarginDefault(
                                           new LeftMargin() { Width = "115" },
                                           new RightMargin() { Width = "115" },
                                           new TopMargin() { Width = "50" },
                                           new BottomMargin() { Width = "50" }
                                           
                                       )
                                   );
                                    table.AppendChild(tblProperties);

                                    #region Gendata table
                                    var o = 1;
                                    foreach (var i in data.DLG.Dlg_2)
                                    {
                                        if (i.Code != "701001")
                                        {
                                            TableRow row = new TableRow();
                                            row.Append(CreateCell("+ " + i.Col1, true, 26, false, "3500"));
                                            row.Append(CreateCell(":", false, 26, true, "1"));
                                            row.Append(CreateCell(i.Col2.Value.ToString("N0"), true, 26, false, "2400"));
                                            row.Append(CreateCell("đ/lít thực tế", false, 26, false, "2400"));
                                            table.Append(row);
                                            o++;
                                        }
                                    }

                                    #endregion

                                    paragraph2.Parent.InsertAfter(table, paragraph2);
                                    paragraph2.Remove();
                                }
                                break;
                        }
                    }
                }
                //}

                //if (code != lstCustomerChecked.LastOrDefault())
                //{
                //    AppendWordFilesToNewDocument(filePathTemplate, fullPath);
                //}
            }
            else if (nameTemp == "QDGNoiDung")
            {
                var dlg5 = data.DLG.Dlg_5;
                using (WordprocessingDocument doc = WordprocessingDocument.Open(fullPath, true))
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    DocumentFormat.OpenXml.Wordprocessing.Body body = mainPart.Document.Body;

                    foreach (var t in lstTextElement)
                    {
                        switch (t)
                        {
                            case "##F_DATE@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, f_date);
                                break;
                            case "##DATE@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, date);
                                break;
                            case "##QUYET_DINH_SO@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.QuyetDinhSo);
                                break;
                            case "##DAI_DIEN@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.DaiDien);
                                break;
                            case "##NGUOI_DAI_DIEN@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.NguoiDaiDien);
                                break;
                            case "##TABLE_ND@@":
                                Paragraph paragraph = body.Descendants<Paragraph>()
                                           .FirstOrDefault(p => p.InnerText.Contains("##TABLE_ND@@"));
                                if (paragraph != null)
                                {
                                    Table table = new Table();
                                    TableProperties tblProperties = new TableProperties(
                                        new TableBorders(
                                            new TopBorder { Val = BorderValues.Single, Size = 4 },
                                            new BottomBorder { Val = BorderValues.Single, Size = 4 },
                                            new LeftBorder { Val = BorderValues.Single, Size = 4 },
                                            new RightBorder { Val = BorderValues.Single, Size = 4 },
                                            new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                                            new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                                        ),
                                        new TableCellMarginDefault(
                                            new LeftMargin() { Width = "115" },
                                            new RightMargin() { Width = "115" },
                                            new TopMargin() { Width = "50" },
                                            new BottomMargin() { Width = "50" }
                                        )
                                    );
                                    table.AppendChild(tblProperties);

                                    #region Header table
                                    TableRow rowHeader = new TableRow();

                                    TableCell cell1 = CreateCell("TT",true, 26,true,"1");
                                    TableCell cell2 = CreateCell("MẶT HÀNG",true, 26,true, "3000");
                                    TableCell cell3 = CreateCell("ĐƠN GIÁ", true, 26, true, "3000");
                                    TableCell cell4 = CreateCell("ĐƠN VỊ TÍNH", true, 26, true, "3000");

                                    rowHeader.Append(cell1);
                                    rowHeader.Append(cell2);
                                    rowHeader.Append(cell3);
                                    rowHeader.Append(cell4);

                                    table.Append(rowHeader);
                                    #endregion

                                    #region Gendata table
                                    var o = 1;
                                    foreach (var i in dlg5)
                                    {
                                        if (i.Code != "701001")
                                        {
                                            TableRow row = new TableRow();
                                            row.Append(CreateCell(i.ColA, true, 26, true, "1"));
                                            row.Append(CreateCell(i.ColB, true, 26, true));
                                            row.Append(CreateCell(i.Col5.Value.ToString("N0"), true, 26));
                                            row.Append(CreateCell("đ/ lít thực tế", true, 26));
                                            table.Append(row);
                                            o++;
                                        }
                                    }
                                    #endregion

                                    paragraph.Parent.InsertAfter(table, paragraph);
                                    paragraph.Remove();
                                }
                                break;
                             
                        }

                    }
                }
            }
            else if (nameTemp == "QDGCtyPTS")
            {
                var dlg6 = data.DLG.Dlg_6;
                using (WordprocessingDocument doc = WordprocessingDocument.Open(fullPath, true))
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    DocumentFormat.OpenXml.Wordprocessing.Body body = mainPart.Document.Body;

                    foreach (var t in lstTextElement)
                    {
                        switch (t)
                        {
                            case "##F_DATE@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, f_date);
                                break;
                            case "##DATE@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, date);
                                break;
                            case "##QUYET_DINH_SO@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.QuyetDinhSo);
                                break;
                            case "##DAI_DIEN@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.DaiDien);
                                break;
                            case "##NGUOI_DAI_DIEN@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.NguoiDaiDien);
                                break;
                            case "##TABLE_PTS@@":
                                Paragraph paragraph = body.Descendants<Paragraph>()
                                           .FirstOrDefault(p => p.InnerText.Contains("##TABLE_PTS@@"));
                                if (paragraph != null)
                                {
                                    Table table = new Table();
                                    TableProperties tblProperties = new TableProperties(
                                        new TableBorders(
                                            new TopBorder { Val = BorderValues.Single, Size = 4 },
                                            new BottomBorder { Val = BorderValues.Single, Size = 4 },
                                            new LeftBorder { Val = BorderValues.Single, Size = 4 },
                                            new RightBorder { Val = BorderValues.Single, Size = 4 },
                                            new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                                            new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                                        ),
                                        new TableCellMarginDefault(
                                            new LeftMargin() { Width = "115" },
                                            new RightMargin() { Width = "115" },
                                            new TopMargin() { Width = "50" },
                                            new BottomMargin() { Width = "50" }
                                        )
                                    );
                                    table.AppendChild(tblProperties);

                                    #region Header table
                                    TableRow rowHeader = new TableRow();

                                    TableCell cell1 = CreateCell("TT", true, 26, true,"1");
                                    TableCell cell2 = CreateCell("MẶT HÀNG", true, 26, true, "3000");
                                    TableCell cell3 = CreateCell("GIÁ BÁN", true, 26, true, "3000");
                                    TableCell cell4 = CreateCell("ĐƠN VỊ TÍNH", true, 26, true, "3000");

                                    rowHeader.Append(cell1);
                                    rowHeader.Append(cell2);
                                    rowHeader.Append(cell3);
                                    rowHeader.Append(cell4);

                                    table.Append(rowHeader);
                                    #endregion

                                    #region Gendata table
                                    var o = 1;
                                    foreach (var i in dlg6)
                                    {
                                        if (i.Code != "701001")
                                        {
                                            TableRow row = new TableRow();
                                            row.Append(CreateCell(i.ColA, true, 26, true, "1"));
                                            row.Append(CreateCell(i.ColB, true, 26, true, "3000"));
                                            row.Append(CreateCell(i.Col6.Value.ToString("N0"), true, 26, true, "3000"));
                                            row.Append(CreateCell("đ/ lít thực tế", true, 26, true, "3000"));
                                            table.Append(row);
                                            o++;
                                        }
                                    }
                                    #endregion

                                    paragraph.Parent.InsertAfter(table, paragraph);
                                    paragraph.Remove();
                                }
                                break;
                        }

                    }
                }
            }
            else if (nameTemp == "QDGBanBuon")
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(fullPath, true))
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    DocumentFormat.OpenXml.Wordprocessing.Body body = mainPart.Document.Body;

                    foreach (var t in lstTextElement)
                    {
                        switch (t)
                        {
                            case "##F_DATE@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, f_date);
                                break;
                            case "##DATE@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, date);
                                break;
                            case "##F_DATE_HOUR@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, f_date_hour);
                                break;
                            case "##QUYET_DINH_SO@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.QuyetDinhSo);
                                break;
                            case "##DAI_DIEN@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.DaiDien);
                                break;
                            case "##NGUOI_DAI_DIEN@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.NguoiDaiDien);
                                break;
                        }

                    }
                }
            }
            else if(nameTemp == "MucGiamGiaNQTM")
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(fullPath, true))
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    DocumentFormat.OpenXml.Wordprocessing.Body body = mainPart.Document.Body;

                    foreach (var t in lstTextElement)
                    {
                        switch (t)
                        {
                            case "##F_DATE@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, f_date);
                                break;
                            case "##DATE@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, date);
                                break;
                            case "##F_DATE_HOUR@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, f_date_hour);
                                break;
                            case "##QUYET_DINH_SO@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.QuyetDinhSo);
                                break;
                            case "##DAI_DIEN@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.DaiDien);
                                break;
                            case "##NGUOI_DAI_DIEN@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.NguoiDaiDien);
                                break;
                        }

                    }
                } 
            }
            else if (nameTemp == "KeKhaiGia")
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(fullPath, true))
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    DocumentFormat.OpenXml.Wordprocessing.Body body = mainPart.Document.Body;

                    foreach (var t in lstTextElement)
                    {
                        switch (t)
                        {
                            case "##F_DATE@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, f_date);
                                break;                          
                            case "##DAI_DIEN@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.DaiDien);
                                break;
                            case "##NGUOI_DAI_DIEN@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.NguoiDaiDien);
                                break;
                        }

                    }
                }
            }
            else if (nameTemp == "ToTrinh")
            {
                var dlg7 = data.DLG.Dlg_7;
                var dlg8 = data.DLG.Dlg_8;
                using (WordprocessingDocument doc = WordprocessingDocument.Open(fullPath, true))
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    DocumentFormat.OpenXml.Wordprocessing.Body body = mainPart.Document.Body;

                    foreach (var t in lstTextElement)
                    {
                        switch (t)
                        {
                         
                            case "##DATE@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, date);
                                break;
                            case "##F_DATE@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, f_date);
                                break;
                            case "##F_DATE_HOURE@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, f_date_hour);
                                break;
                            case "##TABLE_LAI_GOP@@":
                                Paragraph paragraph = body.Descendants<Paragraph>()
                                       .FirstOrDefault(p => p.InnerText.Contains("##TABLE_LAI_GOP@@"));
                                if (paragraph != null)
                                {
                                    Table table = new Table();
                                    TableProperties tblProperties = new TableProperties(
                                        new TableBorders(
                                            new TopBorder { Val = BorderValues.Single, Size = 4 },
                                            new BottomBorder { Val = BorderValues.Single, Size = 4 },
                                            new LeftBorder { Val = BorderValues.Single, Size = 4 },
                                            new RightBorder { Val = BorderValues.Single, Size = 4 },
                                            new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                                            new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                                        )
                                    );
                                    table.AppendChild(tblProperties);

                                    #region Header table


                                    TableRow headerRow1 = new TableRow();
                                    headerRow1.Append(CreateCell("So sánh lãi gộp giữa hai kỳ", true, 26, true, "2082", 4, 1)); 

                                    TableRow headerRow2 = new TableRow();
                                    headerRow2.Append(CreateCell("Mặt hàng", true, 26, true, "2082"));
                                    headerRow2.Append(CreateCell("LG Cũ", true, 26, true, "2082"));
                                    headerRow2.Append(CreateCell("LG Mới", true, 26, true, "2082"));
                                    headerRow2.Append(CreateCell("Tăng/Giảm", true, 26, true, "2082"));

                                    table.Append(headerRow1);
                                    table.Append(headerRow2);

                                    // Thêm dòng tiêu đề "Vùng thị trường trung tâm"
                                    TableRow regionRow1 = new TableRow();
                                    regionRow1.Append(CreateCell("Vùng thị trường trung tâm", true, 26, true, "2082", 4, 1)); 
                                    table.Append(regionRow1);
                                    #endregion

                                    #region Gendata table                                   
                                    foreach (var i in dlg7.Where(x => x.Type == "TT"))
                                    {
                                        if(i.Code != "701001")
                                        {
                                        TableRow row = new TableRow();
                                        row.Append(CreateCell(i.ColA, false, 26, false, "3000")); // Tên mặt hàng
                                        row.Append(CreateCell(i.Col1?.ToString("N0"), false, 26, false, "2082")); // LG cũ
                                        row.Append(CreateCell(i.Col2?.ToString("N0"), false, 26, false, "2082")); // LG mới
                                        row.Append(CreateCell(i.TangGiam1_2?.ToString("N0"), false, 26, false, "2082")); 
                                        table.Append(row);
                                        }    
                                       
                                    }

                                    // Thêm dòng tiêu đề "Các vùng thị trường còn lại"
                                    TableRow regionRow2 = new TableRow();
                                    regionRow2.Append(CreateCell("Các vùng thị trường còn lại", true, 26, true, "2082", 4, 1)); // Gộp 4 cột
                                    table.Append(regionRow2);

                                    // Duyệt danh sách dlg7, in từng mặt hàng thuộc vùng còn lại
                                    foreach (var i in dlg7.Where(x => x.Type == "OTHER"))
                                    {
                                        if (i.Code != "701001")
                                        {
                                            TableRow row = new TableRow();
                                            row.Append(CreateCell(i.ColA, false, 26, false, "3000")); // Tên mặt hàng
                                            row.Append(CreateCell(i.Col1?.ToString("N0"), false, 26, false, "2082")); // LG cũ
                                            row.Append(CreateCell(i.Col2?.ToString("N0"), false, 26, false, "2082")); // LG mới
                                            row.Append(CreateCell(i.TangGiam1_2?.ToString("N0"), false, 26, false, "2082")); 
                                            table.Append(row);
                                        }
                                       
                                    }
                                        #endregion

                                    paragraph.Parent.InsertAfter(table, paragraph);
                                    paragraph.Remove();
                                }
                                break;
                            case "##TABLE_GIAM_GIA@@":
                                Paragraph paragraph1 = body.Descendants<Paragraph>()
                                        .FirstOrDefault(p => p.InnerText.Contains("##TABLE_GIAM_GIA@@"));
                                if (paragraph1 != null)
                                {
                                    Table table = new Table();
                                    TableProperties tblProperties = new TableProperties(
                                        new TableBorders(
                                            new TopBorder { Val = BorderValues.Single, Size = 4 },
                                            new BottomBorder { Val = BorderValues.Single, Size = 4 },
                                            new LeftBorder { Val = BorderValues.Single, Size = 4 },
                                            new RightBorder { Val = BorderValues.Single, Size = 4 },
                                            new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                                            new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                                        )
                                    );
                                    table.AppendChild(tblProperties);

                                    #region Header table
                                    TableRow headerRow1 = new TableRow();
                                    
                                    headerRow1.Append(CreateCell("So sánh lãi gộp giữa hai kỳ", true, 26, true, "2082", 4, 1));

                                    TableRow headerRow2 = new TableRow();
                                    headerRow2.Append(CreateCell("Mặt hàng", true, 26, true, "2082"));
                                    headerRow2.Append(CreateCell("CK Cũ", true, 26, true, "2082"));
                                    headerRow2.Append(CreateCell("CK Mới", true, 26, true, "2082"));
                                    headerRow2.Append(CreateCell("Tăng/Giảm", true, 26, true, "2082"));

                                    table.Append(headerRow1);
                                    table.Append(headerRow2);

                                    #endregion

                                    #region Gendata table
                                    var o = 1;
                                    foreach (var i in dlg8.Where(x => x.Type == "TT"))
                                    {
                                        if (i.Code != "701001")
                                        {
                                            TableRow row = new TableRow();

                                            row.Append(CreateCell(i.ColA, false, 26, false, "3000")); // Tên mặt hàng
                                            row.Append(CreateCell(i.Col1?.ToString("N0"), false, 26, false, "2082")); // LG cũ
                                            row.Append(CreateCell(i.Col2?.ToString("N0"), false, 26, false, "2082")); // LG mới
                                            row.Append(CreateCell(i.TangGiam1_2?.ToString("N0"), false, 26, false, "2082")); 
                                            table.Append(row);
                                        }

                                    }
                                    #endregion

                                    paragraph1.Parent.InsertAfter(table, paragraph1);
                                    paragraph1.Remove();
                                }
                                break;
                        }

                    }
                }
            }
            
            else if (nameTemp == "QDGBanLe")
            {
                var sortedHS2 = model.HS2.OrderBy(x => int.Parse(x.GoodsCode)).ToList();
                using (WordprocessingDocument doc = WordprocessingDocument.Open(fullPath, true))
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    DocumentFormat.OpenXml.Wordprocessing.Body body = mainPart.Document.Body;

                    foreach (var t in lstTextElement)
                    {
                        switch (t)
                        {
                            case "##F_DATE@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, f_date);
                                break;
                            case "##DATE@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, date);
                                break;
                            case "##QUYET_DINH_SO@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.QuyetDinhSo);
                                break;
                            case "##DAI_DIEN@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.DaiDien);
                                break;
                            case "##NGUOI_DAI_DIEN@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, NguoiKyTen.NguoiDaiDien);
                                break;
                            case "##TABLE_TT@@":
                                Paragraph paragraph = body.Descendants<Paragraph>()
                                           .FirstOrDefault(p => p.InnerText.Contains("##TABLE_TT@@"));
                                if (paragraph != null)
                                {
                                    Table table = new Table();
                                    DocumentFormat.OpenXml.Wordprocessing.TableProperties tblProperties = new DocumentFormat.OpenXml.Wordprocessing.TableProperties(
                                       new TableCellMarginDefault(
                                           new LeftMargin() { Width = "115" },
                                           new RightMargin() { Width = "115" },
                                           new TopMargin() { Width = "50" },
                                           new BottomMargin() { Width = "50" }
                                           
                                       )
                                   );
                                    table.AppendChild(tblProperties);

                                    #region Gendata table
                                    var o = 1;
                                    var goodsList = goods.Where(x => x.IsActive == true)
                                                         .OrderByDescending(x => x.ThueBvmt)
                                                         .ToList();
                                    foreach (var i in goodsList)
                                    {
                                        var HS2Item = model.HS2.FirstOrDefault(x => x.GoodsCode == i.Code);
                                        if (HS2Item != null)
                                        {
                                            TableRow row = new TableRow();
                                            row.Append(CreateCell("+ " + i.Name, true, 26, false, "3500"));
                                            row.Append(CreateCell(":", false, 26, true, "1"));
                                            row.Append(CreateCell(HS2Item.Gny.ToString("N0"), true, 26, false, "2400"));
                                            row.Append(CreateCell("đ/lít thực tế", false, 26, false, "2400"));
                                            table.Append(row);
                                            o++;
                                        }
                                    }
                                    #endregion
                                    paragraph.Parent.InsertAfter(table, paragraph);
                                    paragraph.Remove();
                                }
                                break;
                            case "##TABLE_CL@@":
                                Paragraph paragraph2 = body.Descendants<Paragraph>()
                                           .FirstOrDefault(p => p.InnerText.Contains("##TABLE_CL@@"));
                                if (paragraph2 != null)
                                {
                                    Table table = new Table();
                                    DocumentFormat.OpenXml.Wordprocessing.TableProperties tblProperties = new DocumentFormat.OpenXml.Wordprocessing.TableProperties(
                                       new TableCellMarginDefault(
                                           new LeftMargin() { Width = "115" },
                                           new RightMargin() { Width = "115" },
                                           new TopMargin() { Width = "50" },
                                           new BottomMargin() { Width = "50" }
                                           
                                       )
                                   );
                                    table.AppendChild(tblProperties);

                                    #region Gendata table
                                    var o = 1;
                                    foreach (var i in data.DLG.Dlg_2)
                                    {
                                        if(i.Code != "701001")
                                        {
                                            TableRow row = new TableRow();
                                            row.Append(CreateCell("+ " + i.Col1, true, 26, false, "3500"));
                                            row.Append(CreateCell(":", false, 26, true, "1"));
                                            row.Append(CreateCell(i.Col2.Value.ToString("N0"), true, 26, false, "2400"));
                                            row.Append(CreateCell("đ/lít thực tế", false, 26, false, "2400"));
                                            table.Append(row);
                                            o++;
                                        }
                                    }

                                    #endregion

                                    paragraph2.Parent.InsertAfter(table, paragraph2);
                                    paragraph2.Remove();
                                }
                                break;
                        }

                    }
                }
            }
            #endregion






            #endregion

            return $"{folderName}/{fileName}";
        }

        public async Task<string> GenarateWord(List<string> lstCustomerChecked, string headerId)
        {
            #region Tạo 1 file word mới từ file template
            var filePathTemplate = Directory.GetCurrentDirectory() + "/Template/ThongBaoGia.docx";
            var folderName = Path.Combine($"Upload/{DateTime.Now.Year}/{DateTime.Now.Month}");
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fileName = $"{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}_ThongBaoGia.docx";
            var fullPath = Path.Combine(pathToSave, fileName);
            File.Copy(filePathTemplate, fullPath, true);
            #endregion

            #region Lấy các text element
            List<string> lstTextElement = new List<string>();
            WordDocumentService wordDocumentService = new WordDocumentService();
            using (WordprocessingDocument doc = WordprocessingDocument.Open(fullPath, true))
            {
                lstTextElement = wordDocumentService.FindTextElement(doc);
                lstTextElement = lstTextElement.Distinct().ToList();
            }
            #endregion

            #region Fill dữ liệu
            var data = await GetResult(headerId);
            var header = await _dbContext.TblBuCalculateResultList.FindAsync(headerId);
            foreach (var code in lstCustomerChecked)
            {
                var d = data.VK11BB.Where(x => x.Col2 == code).ToList();
                var c = await _dbContext.TblMdCustomer.FindAsync(code);
                using (WordprocessingDocument doc = WordprocessingDocument.Open(fullPath, true))
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    DocumentFormat.OpenXml.Wordprocessing.Body body = mainPart.Document.Body;

                    foreach(var t in lstTextElement)
                    {
                        switch (t)
                        {
                            case "##DATE@@":
                                var text = $"{header.FDate.Hour}h{header.FDate.Minute} ngày {header.FDate.Day} tháng {header.FDate.Month} năm {header.FDate.Year}";
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, text);
                                break;
                            case "##COMPANY@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, c.Name);
                                break;
                            case "##ADDRESS@@":
                                wordDocumentService.ReplaceStringInWordDocumennt(doc, t, c.Address);
                                break;
                            case "##TABLE@@":
                                Paragraph paragraph = body.Descendants<Paragraph>()
                                           .FirstOrDefault(p => p.InnerText.Contains("##TABLE@@"));
                                if (paragraph != null)
                                {
                                    Table table = new Table();
                                    TableProperties tblProperties = new TableProperties(
                                        new TableBorders(
                                            new TopBorder { Val = BorderValues.Single, Size = 4 },
                                            new BottomBorder { Val = BorderValues.Single, Size = 4 },
                                            new LeftBorder { Val = BorderValues.Single, Size = 4 },
                                            new RightBorder { Val = BorderValues.Single, Size = 4 },
                                            new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                                            new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                                        )
                                    );
                                    table.AppendChild(tblProperties);
                                    #region Header table
                                    TableRow rowHeader = new TableRow();


                                    TableCell CreateHeaderCell(string text, int gridSpan, int fontSize = 16)
                                    {
                                        TableCell cell = new TableCell();
                                        Paragraph paragraph = new Paragraph(new Run(new Text(text)));
                                        paragraph.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center });
                                        Run run = paragraph.Elements<Run>().First();
                                        run.RunProperties = new RunProperties(
                                           new Bold(),
                                           new FontSize() { Val = new StringValue(fontSize.ToString()) }
                                       );

                                        cell.Append(paragraph);
                                        TableCellProperties cellProperties = new TableCellProperties(
                                            new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
                                        );
                                        if (gridSpan > 1)
                                        {
                                            cellProperties.Append(new GridSpan() { Val = gridSpan });
                                        }
                                        cell.Append(cellProperties);
                                        return cell;
                                    }


                                    TableCell CreateCell(string text, bool isBold, int fontSize = 26)
                                    {
                                        Run run = new Run(new Text(text));
                                        RunProperties runProperties = new RunProperties(
                                            new FontSize() { Val = new StringValue(fontSize.ToString()) }
                                        );
                                        if (isBold)
                                        {
                                            runProperties.AppendChild(new Bold());
                                        }
                                        run.RunProperties = runProperties;
                                        Paragraph paragraph = new Paragraph(run);
                                        return new TableCell(paragraph);
                                    }
                                    TableCell cell1 = CreateHeaderCell("STT", 1, 26);
                                    TableCell cell2 = CreateHeaderCell("Mặt hàng", 1, 26);
                                    TableCell cell3 = CreateHeaderCell("Điểm giao hàng", 1, 26);
                                    TableCell cell4 = CreateHeaderCell("Đơn giá", 2, 26);
                                    rowHeader.Append(cell1);
                                    rowHeader.Append(cell2);
                                    rowHeader.Append(cell3);
                                    rowHeader.Append(cell4);
                                    table.Append(rowHeader);
                                    #endregion

                                    #region Gendata table
                                    var o = 1;
                                    foreach (var i in d)
                                    {
                                        TableRow row = new TableRow();
                                        row.Append(CreateCell(o.ToString(),false, 26));
                                        row.Append(CreateCell(i.ColD, false, 26));
                                        row.Append(CreateCell(i.ColC, false, 26));
                                        row.Append(CreateCell(i.Col6.HasValue ? i.Col6.Value.ToString("N0") : "0", false, 26));
                                        row.Append(CreateCell("Đ/lít tt", false, 26));
                                        table.Append(row);
                                        o++;
                                    }
                                    #endregion

                                    paragraph.Parent.InsertAfter(table, paragraph);
                                    paragraph.Remove();
                                }
                                break;
                        }
                    }
                }

                if (code != lstCustomerChecked.LastOrDefault())
                {
                    AppendWordFilesToNewDocument(filePathTemplate, fullPath);
                }
            }
            #endregion

            return $"{folderName}/{fileName}";
        }

        public async Task<string> GenarateFile(List<string> lstCustomerChecked, string type, string headerId)
        {

            if (type == "WORD")
            {
                var path = await GenarateWord(lstCustomerChecked, headerId);
                _dbContext.TblBuHistoryDownload.Add(new TblBuHistoryDownload
                {
                    Code = Guid.NewGuid().ToString(),
                    HeaderCode = headerId,
                    Name = path.Replace($"Upload/{DateTime.Now.Year}/{DateTime.Now.Month}/", ""),
                    Type = "docx",
                    Path = path
                });
                await _dbContext.SaveChangesAsync();
                return path;
            }
            if (type == "WORDTRINHKY")
            {
                foreach (var n in lstCustomerChecked)
                {
                    if (n == "KeKhaiGiaChiTiet")
                    {
                        var path = await ExportExcelTrinhKy(headerId);
                        _dbContext.TblBuHistoryDownload.Add(new TblBuHistoryDownload
                        {
                            Code = Guid.NewGuid().ToString(),
                            HeaderCode = headerId,
                            Name = path.Replace($"Upload/{DateTime.Now.Year}/{DateTime.Now.Month}/", ""),
                            Type = "xlsx",
                            Path = path
                        });
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        var path = await GenarateWordTrinhKy(headerId, n);
                        _dbContext.TblBuHistoryDownload.Add(new TblBuHistoryDownload
                        {
                            Code = Guid.NewGuid().ToString(),
                            HeaderCode = headerId,
                            Name = path.Replace($"Upload/{DateTime.Now.Year}/{DateTime.Now.Month}/", ""),
                            Type = "docx",
                            Path = path
                        });
                        await _dbContext.SaveChangesAsync();
                    }
                }
                return null;
            }
           
            else
            {
                var w = await GenarateWord(lstCustomerChecked, headerId);
                var pathWord = Directory.GetCurrentDirectory() + "/" + w;
                Aspose.Words.Document doc = new Aspose.Words.Document(pathWord);
                var folderName = Path.Combine($"Upload/{DateTime.Now.Year}/{DateTime.Now.Month}");
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = $"{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}_ThongBaoGia.pdf";
                var fullPath = Path.Combine(pathToSave, fileName);
                doc.Save(fullPath, SaveFormat.Pdf);

                _dbContext.TblBuHistoryDownload.Add(new TblBuHistoryDownload
                {
                    Code = Guid.NewGuid().ToString(),
                    HeaderCode = headerId,
                    Name = fileName,
                    Type = "pdf",
                    Path = $"{folderName}/{fileName}",
                });
                await _dbContext.SaveChangesAsync();
                return $"{folderName}/{fileName}";
            }
        }
        
        static void AppendWordFilesToNewDocument(string directoryPath, string newWordFilePath)
        {
            using (WordprocessingDocument sourceDocument = WordprocessingDocument.Open(directoryPath, false))
            {
                DocumentFormat.OpenXml.Wordprocessing.Body sourceBody = sourceDocument.MainDocumentPart.Document.Body;

                using (WordprocessingDocument destinationDocument = WordprocessingDocument.Open(newWordFilePath, true))
                {
                    DocumentFormat.OpenXml.Wordprocessing.Body destinationBody = destinationDocument.MainDocumentPart.Document.Body;
                    foreach (var element in sourceBody.Elements())
                    {
                        destinationBody.Append(element.CloneNode(true));
                    }
                    destinationDocument.MainDocumentPart.Document.Save();
                }
            }

        }
        static Table CreateSampleTable()
        {
            Table table = new Table();
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Thick), Size = 8 },
                    new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Thick), Size = 8 },
                    new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Thick), Size = 8 },
                    new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Thick), Size = 8 },
                    new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Thick), Size = 8 },
                    new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Thick), Size = 8 }));
            table.AppendChild(tblProp); TableRow tr = new TableRow();
            TableCell tc1 = new TableCell(new Paragraph(new Run(new Text("Cell 1"))));
            TableCell tc2 = new TableCell(new Paragraph(new Run(new Text("Cell 2"))));
            tr.Append(tc1); tr.Append(tc2);
            table.Append(tr); return table;
        }

    }
}