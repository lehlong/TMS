using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Filter.MD;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.MD
{
    public interface IItemService : IGenericService<TblMdItem, ItemDto>
    {
        Task<IList<ItemDto>> GetAll(ItemGetAllFilter filter);
        Task<byte[]> Export(ItemGetAllFilter filter);
        Task<PagedResponseDto> Search(ItemFilter filter);
    }
    public class ItemService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdItem, ItemDto>(dbContext, mapper), IItemService
    {
        public async Task<PagedResponseDto> Search(ItemFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdItem.Include(x => x.Type).Include(x => x.Group)
                    .Include(x => x.AreaReferences).ThenInclude(x => x.Area).ThenInclude(x => x.PartnerReferences)
                    .Where(x => string.IsNullOrEmpty(filter.TypeCode) || x.TypeCode == filter.TypeCode)
                    .Where(x => string.IsNullOrEmpty(filter.GroupCode) || x.GroupCode == filter.GroupCode)
                    .Where(x => filter.AreaId == null || x.AreaReferences.Any(y => y.AreaId == filter.AreaId))
                    .Where(x => filter.PartnerId == null || x.AreaReferences.Any(y => y.Area.PartnerReferences.Any(z => z.PartnerId == filter.PartnerId)))
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Name.Contains(filter.KeyWord)
                    );
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

        public async Task<IList<ItemDto>> GetAll(ItemGetAllFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdItem.Include(x => x.Type).Include(x => x.Group)
                    .Where(x => string.IsNullOrEmpty(filter.TypeCode) || x.TypeCode == filter.TypeCode)
                    .Where(x => string.IsNullOrEmpty(filter.GroupCode) || x.GroupCode == filter.GroupCode)
                    .Where(x => filter.AreaId == null || x.AreaReferences.Any(y => y.AreaId == filter.AreaId))
                    .AsQueryable();

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

        public async Task<byte[]> Export(ItemGetAllFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdItem.Include(x => x.Group)
                    .Where(x => string.IsNullOrEmpty(filter.TypeCode) || x.TypeCode == filter.TypeCode)
                    .Where(x => string.IsNullOrEmpty(filter.GroupCode) || x.GroupCode == filter.GroupCode)
                    .AsQueryable();

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
    }
}
