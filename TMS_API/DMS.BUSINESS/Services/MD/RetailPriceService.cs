﻿using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.MD;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DMS.BUSINESS.Services.MD.RetailPriceService;

namespace DMS.BUSINESS.Services.MD
{
    public interface IRetailPriceService : IGenericService<TblMdRetailPrice, RetailPriceDto>
    {
        Task<IList<RetailPriceDto>> GetAll(BaseMdFilter filter);
       
        Task<byte[]> Export(BaseMdFilter filter); 
        Task<GblModel> BuildDataCreate(); 
        Task<GblModel> InsertData(GblModel model);
        Task<GblModel> UpdateData(GblModel model);
    }
    public class RetailPriceService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdRetailPrice, RetailPriceDto>(dbContext, mapper), IRetailPriceService
    {
        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdRetailPrice.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                    x.Code.Contains(filter.KeyWord));
                }
                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }
                return await Paging(query, filter);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }
        public async Task<IList<RetailPriceDto>> GetAll(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdRetailPrice.AsQueryable();
                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }
                return await base.GetAllMd(query, filter);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }
        public async Task<GblModel> BuildDataCreate()
        {
            try
            {
                var OldGblList = await _dbContext.TblMdRetailPriceList
                                                .OrderByDescending(x => x.FDate)
                                                .FirstOrDefaultAsync();
                List<TblMdRetailPrice> LstOldGbl;
                if (OldGblList == null || OldGblList.Code == null)
                {
                    LstOldGbl = new List<TblMdRetailPrice>();
                }
                else
                {
                    LstOldGbl = await _dbContext.TblMdRetailPrice
                        .Where(x => x.GbllCode == OldGblList.Code)
                        .ToListAsync();
                }
                //var LstOldGgtd = await _dbContext.TblMdGiaGiaoTapDoan.Where(x => x.GgtdlCode == OldGblList.Code).ToListAsync();
                var lstGoods = await _dbContext.TblMdGoods.Where(x => x.IsActive == true).OrderBy(x => x.Code).ToListAsync();

                var gblModel = new GblModel();


                gblModel.oldHeaderGbl = OldGblList == null ? "" : OldGblList.Code;

                gblModel.GbllHeader.Code = Guid.NewGuid().ToString();
                gblModel.GbllHeader.FDate = DateTime.Now;
                gblModel.GbllHeader.Name = "";
                foreach (var g in lstGoods)
                {
                    var gbl = new TblMdRetailPrice();
                    gbl.Code = Guid.NewGuid().ToString();
                    gbl.GoodsCode = g.Code;
                    gbl.NewPrice = 0;
                    gbl.OldPrice = LstOldGbl.Where(x => x.GbllCode == OldGblList.Code && x.GoodsCode == g.Code).Select(x => x.NewPrice).SingleOrDefault();

                    gblModel.Gbl.Add(gbl);
                }

                return gblModel;
            }
            catch
            {
                return new GblModel();
            }
        }
        public async Task<GblModel> InsertData(GblModel model)
        {
            try
            {
                var exists = await _dbContext.TblMdGiaGiaoTapDoanList
                    .AnyAsync(item => item.FDate > model.GbllHeader.FDate);
                var OldGgtdList = await _dbContext.TblMdRetailPriceList.Where(x => x.Code == model.oldHeaderGbl).FirstOrDefaultAsync();

                OldGgtdList.IsActive = false;
                if (exists)
                {
                    model.oldHeaderGbl = "false";
                    return model;
                }
                else
                {
                    // Không có giá trị nào thỏa mãn
                    _dbContext.TblMdRetailPriceList.Add(model.GbllHeader);
                    _dbContext.TblMdRetailPrice.AddRange(model.Gbl);

                    await _dbContext.SaveChangesAsync();
                    return model;
                }
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }
        public async Task<GblModel> UpdateData(GblModel model)
        {
            try
            {
                var exists = await _dbContext.TblMdRetailPriceList.Where(x => x.Code == model.GbllHeader.Code).Select(x => x.IsActive).FirstOrDefaultAsync();

                if (exists == false)
                {
                    model.oldHeaderGbl = "false";
                    return model;
                }
                else
                {
                    _dbContext.TblMdRetailPriceList.Update(model.GbllHeader);
                    _dbContext.TblMdRetailPrice.UpdateRange(model.Gbl);

                    await _dbContext.SaveChangesAsync();
                    return model;

                }
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }
        public async Task<byte[]> Export(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdRetailPrice.AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x => x.Code.Contains(filter.KeyWord));
                }
                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }
                var data = await base.GetAllMd(query, filter);
                int i = 1;
                data.ForEach(x =>
                {
                    x.OrdinalNumber = i++;
                });
                return await ExportExtension.ExportToExcel(data);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }
        public class GblModel
        {
            public string? oldHeaderGbl { set; get; }
            public TblMdRetailPriceList? GbllHeader { get; set; } = new TblMdRetailPriceList();
            public List<TblMdRetailPrice?> Gbl { get; set; } = new List<TblMdRetailPrice>();

        }
    }
}
