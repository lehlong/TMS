﻿using AutoMapper;
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

namespace DMS.BUSINESS.Services.BU
{
    public interface ICalculateResultService : IGenericService<TblMdGoods, GoodsDto>
    {
        Task<CalculateResultModel> GetResult(QueryModel model);
    }
    public class CalculateResultService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdGoods, GoodsDto>(dbContext, mapper), ICalculateResultService
    {
        public async Task<CalculateResultModel> GetResult(QueryModel model)
        {
            try
            {
                var data = new CalculateResultModel();
                var lstGoods = await _dbContext.TblMdGoods.OrderBy(x => x.Code).ToListAsync();
                data.lstGoods = lstGoods;
                var lstMarket = await _dbContext.TblMdMarket.OrderBy(x => x.Code).ToListAsync();
                var lstLGDT = await _dbContext.TblMdLaiGopDieuTiet.ToListAsync();
                var dataVCL = await _dbContext.TblInVinhCuaLo.ToListAsync();
                #region DLG
                foreach(var g in lstGoods)
                {
                    var vcl = dataVCL.Where(x => x.GoodsCode == g.Code).ToList();
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
                        };
                        foreach (var _l in lstGoods)
                        {
                            var _c = lstLGDT.Where(x => x.MarketCode == m.Code && x.GoodsCode == _l.Code);
                            i.LG.Add(_c == null || _c.Count() == 0 ? 0 : _c.Sum(x => x.Price));
                        }
                        data.PT.Add(i);
                        orderPT++;
                    }
                }
                #endregion
                return data;
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return new CalculateResultModel();
            }
        }
    }
}
