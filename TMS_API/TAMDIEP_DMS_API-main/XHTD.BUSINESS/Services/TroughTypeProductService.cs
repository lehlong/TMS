using AutoMapper;
using Common;
using XHTD.BUSINESS.Common;
using XHTD.BUSINESS.Dtos;
using XHTD.CORE.Entities;

namespace XHTD.BUSINESS.Services
{
    public interface ITroughTypeProductService : IXHTDGenericService<TblTroughTypeProduct, TroughTypeProductDto>
    {
        Task<IList<TroughTypeProductDto>> GetAll(BaseMdFilter filter);

        Task<byte[]> Export(BaseMdFilter filter);
    }
    public class TroughTypeProductService(XHTDDbContext dbContext, IMapper mapper) : XHTDGenericService<TblTroughTypeProduct, TroughTypeProductDto>(dbContext, mapper), ITroughTypeProductService
    {
        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblTroughTypeProducts.AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.TroughCode.Contains(filter.KeyWord)
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

        public async Task<IList<TroughTypeProductDto>> GetAll(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblTroughTypeProducts.AsQueryable();

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
                var query = _dbContext.TblTroughTypeProducts.AsQueryable();


                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.TroughCode.Contains(filter.KeyWord)
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
