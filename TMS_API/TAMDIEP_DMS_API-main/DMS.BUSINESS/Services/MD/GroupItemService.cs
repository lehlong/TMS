using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.CORE;
using DMS.CORE.Entities.MD;

namespace DMS.BUSINESS.Services.MD
{
    public interface IGroupItemService : IGenericService<TblMdGroupItem, GroupItemDto>
    {
        Task<IList<GroupItemDto>> GetAll(BaseMdFilter filter);

        Task<byte[]> Export(BaseMdFilter filter);
    }
    public class GroupItemService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdGroupItem, GroupItemDto>(dbContext, mapper), IGroupItemService
    {
        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdGroupItem.AsQueryable();
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

        public async Task<IList<GroupItemDto>> GetAll(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdGroupItem.AsQueryable();

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
                var query = _dbContext.TblMdGroupItem.AsQueryable();


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
