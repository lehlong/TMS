using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Filter.MD;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.MD
{
    public interface IVehicleService : IGenericService<TblMdVehicle, VehicleDto>
    {
        Task<IList<VehicleDto>> GetAll(VehicleGetAllFilter filter);
        Task<PagedResponseDto> Search(VehicleFilter filter);
    }
    public class VehicleService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdVehicle, VehicleDto>(dbContext, mapper), IVehicleService
    {
        public async Task<PagedResponseDto> Search(VehicleFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdVehicle.Include(x => x.Type).Include(x => x.PartnerCreate)
                    .Include(x => x.PartnerReferences)
                    .Include(x => x.DriverReferences).ThenInclude(x => x.Driver)
                    .Where(x => filter.PartnerId == null || x.PartnerReferences.Any(y => y.PartnerId == filter.PartnerId))
                    .Where(x => string.IsNullOrEmpty(filter.VehicleTypeCode) || x.VehicleTypeCode == filter.VehicleTypeCode)
                    .AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Code.Contains(filter.KeyWord)
                    );
                }

                if (filter.PartnerIdCreate.HasValue)
                {
                    query = query.Where(x => x.PartnerIdCreate == filter.PartnerIdCreate);
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

        public async Task<IList<VehicleDto>> GetAll(VehicleGetAllFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdVehicle.Include(x => x.Type).Include(x=>x.PartnerReferences)
                    .Where(x => filter.PartnerId == null || x.PartnerReferences.Any(y => y.PartnerId == filter.PartnerId))
                    .Where(x => string.IsNullOrEmpty(filter.VehicleTypeCode) || x.VehicleTypeCode == filter.VehicleTypeCode)
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
        public async override Task Update(IDto dto)
        {
            if (dto is VehicleCreateUpdateDto model)
            {
                var currentObj = await _dbContext.TblMdVehicle.Include(x => x.PartnerReferences).FirstOrDefaultAsync(x => x.Code == model.Code);
                await base.UpdateWithListInside(dto, currentObj);
            }
        }

        public async override Task<VehicleDto> GetById(object id)
        {
            var data = await _dbContext.TblMdVehicle.Include(x => x.Type)
                .Include(x => x.PartnerReferences).ThenInclude(x => x.Partner)
                    .FirstOrDefaultAsync(x => x.Code == (string)id);

            return _mapper.Map<VehicleDto>(data);
        }
    }
}
