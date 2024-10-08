using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Filter.MD;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.MD
{
    public interface ISaleTypeService : IGenericService<TblMdSaleType, SaleTypeDto>
    {
        Task<IList<SaleTypeDto>> GetAll(SaleTypeGetAllFilter filter);
    }
    public class SaleTypeService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdSaleType, SaleTypeDto>(dbContext, mapper), ISaleTypeService
    {
        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdSaleType
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

        public async Task<IList<SaleTypeDto>> GetAll(SaleTypeGetAllFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdSaleType
                    .AsQueryable();

                if (filter.IsSupplier.HasValue)
                {
                    query = query.Where(x => x.Code.Contains("CH"));
                }

                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }

                return _mapper.Map<IList<SaleTypeDto>>(await query.ToListAsync());
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
