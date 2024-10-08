using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Filter.MD;
using DMS.CORE;
using DMS.CORE.Entities.MD;

namespace DMS.BUSINESS.Services.MD
{
    public interface IWardService : IGenericService<TblMdWard, WardDto>
    {
        Task<IList<WardDto>> GetAll(WardGetAllFilter filter);
        Task<PagedResponseDto> Search(WardFilter filter);
        Task<byte[]> Export(BaseMdFilter filter);
    }
    public class WardService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdWard, WardDto>(dbContext, mapper), IWardService
    {
        public async Task<PagedResponseDto> Search(WardFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdWard
                    .Where(x => filter.DistrictId == null || x.DistrictId == filter.DistrictId)
                    .AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Name.Contains(filter.KeyWord)
                    );
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

        public async Task<IList<WardDto>> GetAll(WardGetAllFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdWard
                    .Where(x => filter.DistrictId == null || x.DistrictId == filter.DistrictId)
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

        public async Task<byte[]> Export(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdWard.AsQueryable();

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
