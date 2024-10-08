using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Filter.MD;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.MD
{
    public interface IDriverService : IGenericService<TblMdDriver, DriverDto>
    {
        Task<IList<DriverDto>> GetAll(DriverGetAllFilter filter);
        Task<byte[]> Export(BaseMdFilter filter);
        Task<PagedResponseDto> Search(DriverFilter filter);
    }
    public class DriverService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdDriver, DriverDto>(dbContext, mapper), IDriverService
    {
        public async Task<PagedResponseDto> Search(DriverFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdDriver
                    .Include(x => x.Account)
                    .Include(x=>x.VehicleReferences)
                    .ThenInclude(x=>x.Vehicle)
                    .Where(x=> string.IsNullOrEmpty(filter.VehicleCode) || x.VehicleReferences.Any(y=>y.VehicleCode == filter.VehicleCode))
                    .Where(x=> filter.PartnerIdCreate == null || x.PartnerIdCreate == filter.PartnerIdCreate)
                    .AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.UserName.Contains(filter.KeyWord) || x.FullName.Contains(filter.KeyWord)
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

        public async Task<IList<DriverDto>> GetAll(DriverGetAllFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdDriver.Include(x=>x.VehicleReferences)
                    .Where(x => filter.PartnerIdCreate == null || x.PartnerIdCreate == filter.PartnerIdCreate)
                    .Where(x => string.IsNullOrEmpty(filter.VehicleCode) || x.VehicleReferences.Any(y => y.VehicleCode == filter.VehicleCode))
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
                var query = _dbContext.TblMdDriver.Include(x => x.Account).AsQueryable();

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

        public override async Task<DriverDto> GetById(object id)
        {
            var data = await _dbContext.TblMdDriver
                .Include(x => x.VehicleReferences).ThenInclude(x => x.Vehicle).ThenInclude(x => x.Type)
                .Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == (int)id);

            return _mapper.Map<DriverDto>(data);
        }

        public override async Task Update(IDto dto)
        {
            if (dto is not DriverUpdateDto model)
            {
                this.Status = false;
                this.MessageObject.Code = "0000";
                return;
            }
            var currentObj = await _dbContext.TblMdDriver.Include(x => x.VehicleReferences).FirstOrDefaultAsync(x => x.Id == model.Id);
            await base.UpdateWithListInside(dto, currentObj);
        }
    }
}
