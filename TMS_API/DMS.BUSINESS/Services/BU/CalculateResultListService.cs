﻿using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.BU;
using DMS.BUSINESS.Dtos.MD;
using DMS.CORE;
using DMS.CORE.Entities.BU;
using DMS.CORE.Entities.IN;
using DMS.CORE.Entities.MD;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Services.BU
{
    public interface ICalculateResultListService : IGenericService<TblBuCalculateResultList, CalculateResultListDto>
    {
        Task<IList<CalculateResultListDto>> GetAll(BaseMdFilter filter);
        Task<byte[]> Export(BaseMdFilter filter);
        Task InsertData(CalculateResultListDto c);
        Task<InsertModel> GetObjectCreate();
    }
    public class CalculateResultListService(AppDbContext dbContext, IMapper mapper) : GenericService<TblBuCalculateResultList, CalculateResultListDto>(dbContext, mapper), ICalculateResultListService
    {
        public async Task InsertData(CalculateResultListDto c)
        {
            try
            {
                c.Code = Guid.NewGuid().ToString();
                c.Status = "01";
                await this.Add(c);

            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
            }
        }
        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblBuCalculateResultList.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                    x.Name.Contains(filter.KeyWord));
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
        public async Task<IList<CalculateResultListDto>> GetAll(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblBuCalculateResultList.AsQueryable();
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
        public async Task<byte[]> Export(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblBuCalculateResultList.AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x => x.Name.Contains(filter.KeyWord));
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

        public async Task<InsertModel> GetObjectCreate()
        {
            try
            {
                var obj = new InsertModel();
                obj.Header.Code = Guid.NewGuid().ToString();
                obj.Header.IsActive = true;
                obj.Header.FDate = DateTime.Now;
                obj.Header.CreateDate = DateTime.Now;
                obj.Header.UpdateDate = DateTime.Now;
                obj.Header.IsDeleted = false;
                obj.Header.DeleteDate = DateTime.Now;
                obj.Header.Name = "";
                obj.Header.Status = "01";
                var lstGoods = await _dbContext.TblMdGoods.OrderBy(x => x.CreateDate).ToListAsync();
                foreach (var g in lstGoods)
                {
                    obj.HS1.Add(new TblInHeSoMatHang
                    {
                        Code = Guid.NewGuid().ToString(),
                        GoodsCode = g.Code,
                        IsActive = true,
                        FromDate = DateTime.Now,
                        ToDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    });
                    obj.HS2.Add(new TblInVinhCuaLo
                    {
                        Code = Guid.NewGuid().ToString(),
                        GoodsCode = g.Code,
                        IsActive = true,
                        FromDate = DateTime.Now,
                        ToDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    });
                }

                return obj;
            }
            catch
            {
                return new InsertModel();
            }
        }

    }

    public class InsertModel
    {
        public TblBuCalculateResultList Header { get; set; } = new TblBuCalculateResultList();
        public List<TblInHeSoMatHang> HS1 { get; set; } = new List<TblInHeSoMatHang>();
        public List<TblInVinhCuaLo> HS2 { get; set; } = new List<TblInVinhCuaLo>();
    }
}