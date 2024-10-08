using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Filter.MD;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.MD
{
    public interface IDistrictService : IGenericService<TblMdDistrict, DistrictDto>
    {
        Task<IList<DistrictDto>> GetAll(DistrictGetAllFilter filter);
        Task<PagedResponseDto> Search(DistrictFilter filter);
        Task<byte[]> Export(BaseMdFilter filter);
    }
    public class DistrictService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdDistrict, DistrictDto>(dbContext, mapper), IDistrictService
    {
        public async Task<PagedResponseDto> Search(DistrictFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdDistrict.Include(x => x.Wards)
                    .Where(x => filter.ProvineId == null || x.ProvineId == filter.ProvineId)
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

        public async Task<IList<DistrictDto>> GetAll(DistrictGetAllFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdDistrict
                    .Where(x => filter.ProvineId == null || x.ProvineId == filter.ProvineId)
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
                var query = _dbContext.TblMdDistrict.Include(x => x.Wards).AsQueryable();

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
