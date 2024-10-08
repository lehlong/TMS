using AutoMapper;
using Common;
using XHTD.BUSINESS.Common;
using XHTD.BUSINESS.Dtos;
using XHTD.CORE.Entities;

namespace XHTD.BUSINESS.Services
{
    public interface IRfidService : IXHTDGenericService<TblRfid, RfidDto>
    {
        Task<IList<RfidDto>> GetAll(BaseMdFilter filter);

        Task<byte[]> Export(BaseMdFilter filter);
    }
    public class RfidService(XHTDDbContext dbContext, IMapper mapper) : XHTDGenericService<TblRfid, RfidDto>(dbContext, mapper), IRfidService
    {
        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblRfids.AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Code.Contains(filter.KeyWord)
                        || x.Vehicle.Contains(filter.KeyWord)
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

        public async Task<IList<RfidDto>> GetAll(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblRfids.AsQueryable();

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
                var query = _dbContext.TblRfids.AsQueryable();


                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Code.Contains(filter.KeyWord)
                        || x.Vehicle.Contains(filter.KeyWord)
                    );
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
