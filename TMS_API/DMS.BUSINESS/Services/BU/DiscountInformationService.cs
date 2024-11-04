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
                var lstGoods = await _dbContext.TblMdGoods.OrderBy(x => x.CreateDate).ToListAsync();
                data.lstGoods = lstGoods;
                var lstMarket = await _dbContext.TblMdMarket.OrderBy(x => x.Code).ToListAsync();

                var orderMarket = 1;
                var plxna = 1300;
                var apCk = 990;
                var vaCk = 890;
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
                        col1 = m.Gap,

                        col3 = m.Gap + 120,
                        col4 = m.CuocVCBQ,

                        col6 = m.CuocVCBQ + 200,
                    };
                    foreach (var g in lstGoods)
                    {
                        var ck = new CK
                        {
                            plxna = plxna - m.CuocVCBQ,
                        };

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
