using AutoMapper;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.MD;
using DMS.BUSINESS.Models;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Services.BU
{
    public interface IDiscountInformationService : IGenericService<TblMdGoods, GoodsDto>
    {
        Task<DiscountInformationModel> getAll();
    }
    public class DiscountInformationService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdGoods, GoodsDto> (dbContext, mapper), IDiscountInformationService
    {
        public async Task<DiscountInformationModel> getAll()
        {
            try
            {
                var data = new DiscountInformationModel();
                var lstMarket = await _dbContext.TblMdMarket.OrderBy(x => x.Code).ToListAsync();
                var lstDiscountCompetitor = await _dbContext.TblInDiscountCompetitor.OrderBy(x => x.Code).ToListAsync();
                var lstMarketCompetitor = await _dbContext.TblMdMarketCompetitor.OrderBy(x => x.Code).ToListAsync();
                var lstGoods = await _dbContext.TblMdGoods.OrderBy(x => x.CreateDate).ToListAsync();
                data.lstGoods = lstGoods;
                var lstCompetitor = await _dbContext.TblMdCompetitor.OrderBy(x => x.Code).ToListAsync();
                data.lstCompetitor = lstCompetitor;

                var orderMarket = 1;
                var plxna = 1300;
                var ap95 = 990;
                var vaCk = 890;
                var z11 = 1961;

                var row1 = new discout
                {
                    colA = "I",
                    colB = "KHO TRUNG TÂM (FOB)",
                };

                foreach (var g in lstGoods)
                {
                    var ck = new CK
                    {
                        plxna = plxna,
                    };

                    row1.CK.Add(ck);
                }

                data.discount.Add(row1);

                data.discount.Add(new discout
                {
                    colA = "II",
                    colB = "KHO KHÁCH HÀNG (CIF)"
                    
                });

                foreach (var m in lstMarket)
                {
                    var d = new discout
                    {
                        colA = orderMarket.ToString(),
                        colB = m.Name,
                        col1 = m.Gap ?? 0,
                        col4 = m.CuocVCBQ ?? 0,
                    };
                    

                    foreach (var c in lstCompetitor)
                    {
                        var gap = lstMarketCompetitor.Where(x => x.CompetitorCode == c.Code && x.MarketCode == m.Code).Sum(x => x.Gap == 0 ? m.Gap + 120 : x.Gap);
                        var cuocVc = lstMarketCompetitor.Where(x => x.CompetitorCode == c.Code && x.MarketCode == m.Code).Sum(x => x.Gap == 0 ? m.CuocVCBQ + 200 : x.Gap * (decimal)z11 / 1000 ?? 0);
                        d.gaps.Add(Math.Round(gap ?? 0));
                        d.cuocVCs.Add(cuocVc != null ? Math.Round((decimal)cuocVc, 0) : 0);
                    }
                    foreach (var g in lstGoods)
                    {
                        var ck = new CK
                        {
                            plxna = plxna - m.CuocVCBQ,
                        };
                        foreach (var c in lstCompetitor)
                        {
                            var dt = new DT();
                            
                            var gap = lstMarketCompetitor.Where(x => x.CompetitorCode == c.Code && x.MarketCode == m.Code).Sum(x => x.Gap == 0 ? m.Gap + 120 : x.Gap);
                            var ck1 = lstDiscountCompetitor.Where(x => x.CompetitorCode == c.Code && x.GoodsCode == g.Code).Sum(x => x.Discount ?? 0 - gap) ;
                            dt.ckCl.Add(ck1);
                            dt.ckCl.Add(ck1 - (plxna - m.CuocVCBQ));
                            
                            ck.DT.Add(dt);
                        }
                        d.CK.Add(ck);
                    }
                    data.discount.Add(d);
                    orderMarket++;
                }
                return data;
            }
            catch(Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return new DiscountInformationModel();
            }
        }


    }
}
