using AutoMapper;
using Common;
using Microsoft.EntityFrameworkCore;
using XHTD.BUSINESS.Common;
using XHTD.BUSINESS.Dtos;
using XHTD.CORE.Entities;

namespace XHTD.BUSINESS.Services
{
    public interface ITroughService : IXHTDGenericService<TblTrough, TroughDto>
    {
        Task<IList<TroughDto>> GetAll(BaseMdFilter filter);

        Task<byte[]> Export(BaseMdFilter filter);

        Task UpdateTypeproduct(IDto dto);
    }
    public class TroughService(XHTDDbContext dbContext, IMapper mapper) : XHTDGenericService<TblTrough, TroughDto>(dbContext, mapper), ITroughService
    {
        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblTroughs
                            .Include(x => x.TypeProductReferences).ThenInclude(x => x.TypeProductITem)
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

        public async Task<IList<TroughDto>> GetAll(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblTroughs.AsQueryable();

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
                var query = _dbContext.TblTroughs.AsQueryable();


                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Name.Contains(filter.KeyWord)
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

        public async Task UpdateTypeproduct(IDto trough)
        {
            if (trough is not TroughUpdateTypeProductDto model)
            {
                this.Status = false;
                this.MessageObject.Code = "0000";
                return;
            }

            var currentObj = await _dbContext.TblTroughs.Include(x => x.TypeProductReferences).FirstOrDefaultAsync(x => x.Code == model.Code);
            await base.UpdateWithListInside(trough, currentObj);
        }
    }
}
