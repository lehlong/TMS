using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Filter.MD;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.MD
{
    public interface IAreaService : IGenericService<TblMdArea, AreaDto>
    {
        Task<IList<AreaDto>> GetAll(AreaGetAllFilter filter);
        Task<byte[]> Export(AreaGetAllFilter filter);
        Task<PagedResponseDto> Search(AreaFilter filter);
    }
    public class AreaService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdArea, AreaDto>(dbContext, mapper), IAreaService
    {
        public async Task<PagedResponseDto> Search(AreaFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdArea.Include(x=>x.PartnerReferences).Include(x=>x.ItemReferences)
                    .Where(x => filter.PartnerId == null || x.PartnerReferences.Any(y=>y.PartnerId == filter.PartnerId))
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

        public async Task<IList<AreaDto>> GetAll(AreaGetAllFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdArea.Include(x => x.PartnerReferences).Include(x=>x.ItemReferences)
                    .Where(x => filter.PartnerId == null || x.PartnerReferences.Any(y => y.PartnerId == filter.PartnerId)).AsQueryable();

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

        public async Task<byte[]> Export(AreaGetAllFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdArea.Include(x => x.PartnerReferences)
                    .Where(x => filter.PartnerId == null || x.PartnerReferences.Any(y => y.PartnerId == filter.PartnerId)).AsQueryable();

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

        public override async Task Update(IDto dto)
        {
            if(dto is AreaCreateUpdateDto model)
            {
                var currentObj = await _dbContext.TblMdArea
                    .Include(x => x.ItemReferences)
                    .FirstOrDefaultAsync(x=>x.Id == model.Id);
                 await base.UpdateWithListInside(dto,currentObj);
            }
        }
    }
}
