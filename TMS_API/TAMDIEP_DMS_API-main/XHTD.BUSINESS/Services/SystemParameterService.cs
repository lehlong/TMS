using AutoMapper;
using Common;
using XHTD.BUSINESS.Common;
using XHTD.BUSINESS.Dtos;
using XHTD.CORE.Entities;

namespace XHTD.BUSINESS.Services
{
    public interface ISystemParameterService : IXHTDGenericService<TblSystemParameter, SystemParameterDto>
    {
        Task<IList<SystemParameterDto>> GetAll(BaseMdFilter filter);

        Task<byte[]> Export(BaseMdFilter filter);
    }
    public class SystemParameterService(XHTDDbContext dbContext, IMapper mapper) : XHTDGenericService<TblSystemParameter, SystemParameterDto>(dbContext, mapper), ISystemParameterService
    {
        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblSystemParameters.AsQueryable();
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

        public async Task<IList<SystemParameterDto>> GetAll(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblSystemParameters.AsQueryable();

                return await base.GetAllMd(query,filter);
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
                var query = _dbContext.TblSystemParameters.AsQueryable();


                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Name.Contains(filter.KeyWord)
                    );
                }

                var data = await base.GetAllMd(query,filter);

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
