using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Filter.MD;
using DMS.BUSINESS.Filter.MN;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.MD
{
    public interface IPartnerService : IGenericService<TblMdPartner, PartnerDto>
    {
        Task<IList<PartnerDto>> GetAll(PartnerGetAllFilter filter);
        Task<PagedResponseDto> Search(PartnerFilter filter);
        Task UpdateArea(IDto dto);
        Task<PagedResponseDto> GetByASO(AccountSaleOfficeFilter filter);
        Task<PagedResponseDto> GetWithTypeC2C3(AccountSaleOfficeFilter filter);
        Task<byte[]> ExportWithASO(AccountSaleOfficeFilter filter);
    }
    public class PartnerService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdPartner, PartnerDto>(dbContext, mapper), IPartnerService
    {
        public async Task<PagedResponseDto> Search(PartnerFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdPartner
                    .Include(x => x.Provine)
                    .Include(x => x.District)
                    .Include(x => x.Ward)
                    .Include(x => x.SaleType)
                    .Include(x => x.AreaReferences).ThenInclude(x => x.Area)
                    .Include(x => x.ReferenceBuys)
                    .Include(x => x.ReferenceSells)
                    .Where(x => filter.ProvineId == null || x.ProvineId == filter.ProvineId)
                    .Where(x => filter.DistrictId == null || x.DistrictId == filter.DistrictId)
                    .Where(x => filter.WardId == null || x.WardId == filter.WardId)
                    .Where(x => filter.IsCustomer == null || x.IsCustomer == filter.IsCustomer)
                    .Where(x => filter.IsSupplier == null || x.IsSupplier == filter.IsSupplier)
                    .Where(x => string.IsNullOrEmpty(filter.SaleType) || x.SaleTypeCode == filter.SaleType)
                    .Where(x => filter.SupplierId == null || x.ReferenceSells.Any(y => y.PartnerIdBuy == filter.SupplierId))
                    .Where(x => filter.CustomerId == null || x.ReferenceBuys.Any(y => y.PartnerIdSell == filter.CustomerId))
                    .Where(x => filter.ReferenceId == null || x.ReferenceSells.Any(y => y.PartnerIdBuy == filter.ReferenceId) || x.ReferenceBuys.Any(y => y.PartnerIdSell == filter.ReferenceId))
                    .Where(x=> filter.ExcludePartner == null || filter.ExcludePartner.Count == 0 || !filter.ExcludePartner.Contains(x.Id))
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Code.Contains(filter.KeyWord) || x.Name.Contains(filter.KeyWord)
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

        public async override Task<PartnerDto> GetById(object id)
        {
            var data = await _dbContext.TblMdPartner
                                .Include(x => x.Provine)
                                .Include(x => x.District)
                                .Include(x => x.Ward)
                                .Include(x => x.SaleType)
                                .Include(x => x.ReferenceSells).ThenInclude(x => x.PartnerBuy)
                                .ThenInclude(x => x.AreaReferences).ThenInclude(x => x.Area).ThenInclude(x => x.ItemReferences).ThenInclude(x => x.Item)
                                .Include(x => x.ReferenceBuys).ThenInclude(x => x.PartnerSell)
                                .ThenInclude(x => x.AreaReferences).ThenInclude(x => x.Area).ThenInclude(x => x.ItemReferences).ThenInclude(x => x.Item)
                                .Include(x => x.VehicleReferences).ThenInclude(x => x.Vehicle).ThenInclude(x => x.Type)
                                .Include(x => x.VehicleReferences).ThenInclude(x => x.Vehicle).ThenInclude(x => x.PartnerCreate)
                                .Include(x => x.AreaReferences).ThenInclude(x => x.Area).ThenInclude(x => x.ItemReferences).ThenInclude(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == (int)id);

            return _mapper.Map<PartnerDto>(data);
        }

        public async Task<IList<PartnerDto>> GetAll(PartnerGetAllFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdPartner.Include(x => x.ReferenceBuys).Include(x => x.ReferenceSells)
                    .Where(x => filter.IsCustomer == null || x.IsCustomer == filter.IsCustomer)
                    .Where(x => filter.IsSupplier == null || x.IsSupplier == filter.IsSupplier)
                    .Where(x => string.IsNullOrEmpty(filter.SaleType) || x.SaleTypeCode == filter.SaleType)
                    .Where(x => filter.ReferenceId == null || x.ReferenceSells.Any(y => y.PartnerIdBuy == filter.ReferenceId) || x.ReferenceBuys.Any(y => y.PartnerIdSell == filter.ReferenceId))
                    .Where(x => filter.IsSaleTypeC2C3 == null || 
                    (filter.IsSaleTypeC2C3 == true ? (x.SaleTypeCode == "CH-C2" || x.SaleTypeCode == "CH-C3") : (x.SaleTypeCode == "NPP")))
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
            if (dto is PartnerUpdateDto model)
            {
                var currentObj = await _dbContext.TblMdPartner
                       .Include(x => x.ReferenceBuys)
                       .Include(x => x.ReferenceSells)
                       .Include(x => x.VehicleReferences)
                       .Include(x => x.AreaReferences)
                       .FirstOrDefaultAsync(x => x.Id == model.Id);
                await base.UpdateWithListInside(dto, currentObj);
            }
        }

        public override async Task<PartnerDto> Add(IDto dto)
        {
            if (dto is PartnerCreateDto model)
            {
                if (await _dbContext.TblMdPartner.AnyAsync(x => x.Code == model.Code))
                {
                    this.Status = false;
                    this.MessageObject.Code = "0004";
                    return null;
                }
            }

            return await base.Add(dto);
        }

        public async Task UpdateArea(IDto dto)
        {
            if (dto is PartnerUpdateAreaDto model)
            {
                var currentObj = await _dbContext.TblMdPartner
                       .Include(x => x.AreaReferences)
                       .FirstOrDefaultAsync(x => x.Id == model.Id);
                await base.UpdateWithListInside(dto, currentObj);
            }
        }

        public async Task<PagedResponseDto> GetByASO(AccountSaleOfficeFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdPartner
                    .Include(x => x.Provine)
                    .Include(x => x.District)
                    .Include(x => x.Ward)
                    .Include(x => x.SaleType)
                    .Include(x => x.AreaReferences).ThenInclude(x => x.Area)
                    .Include(x => x.ReferenceBuys)
                    .Include(x => x.ReferenceSells)
                    .Where(x => string.IsNullOrEmpty(filter.SaleType) || x.SaleTypeCode == filter.SaleType)
                    .Where(x => filter.SupplierId == null || x.ReferenceSells.Any(y => y.PartnerIdSell == filter.SupplierId))
                    .Where(x => filter.ProvineId == null || x.ProvineId == filter.ProvineId)
                    .Where(x => filter.DistrictId == null || x.DistrictId == filter.DistrictId)
                    .Where(x => filter.WardId == null || x.WardId == filter.WardId)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.UserName))
                {
                    query = query.Where(x => x.AccountSaleOffices.Any(y => y.UserName == filter.UserName));
                }

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Code.Contains(filter.KeyWord) || x.Name.Contains(filter.KeyWord)
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

        public async Task<PagedResponseDto> GetWithTypeC2C3(AccountSaleOfficeFilter filter)
        {
            try
            {
                var partnerIds = await _dbContext.TblMnAccountSaleOffices.Where(a => a.UserName == filter.UserName)
                    .Select(a => a.PartnerId).ToListAsync();
                var query = _dbContext.TblMdPartner
                    .Include(x => x.Provine)
                    .Include(x => x.District)
                    .Include(x => x.Ward)
                    .Include(x => x.SaleType)
                    .Include(x => x.AreaReferences).ThenInclude(x => x.Area)
                    .Include(x => x.ReferenceBuys)
                    .Include(x => x.ReferenceSells)
                    .Where(x => !partnerIds.Contains(x.Id))
                    .Where(x => x.SaleType.Code == "CH-C2" || x.SaleType.Code == "CH-C3")
                    .Where(x => filter.SupplierId == null || x.ReferenceSells.Any(y => y.PartnerIdSell == filter.SupplierId))
                    .Where(x => filter.ProvineId == null || x.ProvineId == filter.ProvineId)
                    .Where(x => filter.DistrictId == null || x.DistrictId == filter.DistrictId)
                    .Where(x => filter.WardId == null || x.WardId == filter.WardId)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Code.Contains(filter.KeyWord) || x.Name.Contains(filter.KeyWord)
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

        public async Task<byte[]> ExportWithASO(AccountSaleOfficeFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdPartner
                   .Include(x => x.Provine)
                   .Include(x => x.District)
                   .Include(x => x.Ward)
                   .Include(x => x.SaleType)
                   .Include(x => x.AreaReferences).ThenInclude(x => x.Area)
                   .Include(x => x.ReferenceBuys)
                   .Include(x => x.ReferenceSells)
                   .Where(x => string.IsNullOrEmpty(filter.SaleType) || x.SaleTypeCode == filter.SaleType)
                   .Where(x => filter.SupplierId == null || x.ReferenceSells.Any(y => y.PartnerIdSell == filter.SupplierId))
                   .Where(x => filter.ProvineId == null || x.ProvineId == filter.ProvineId)
                   .Where(x => filter.DistrictId == null || x.DistrictId == filter.DistrictId)
                   .Where(x => filter.WardId == null || x.WardId == filter.WardId)
                   .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.UserName))
                {
                    query = query.Where(x => x.AccountSaleOffices.Any(y => y.UserName == filter.UserName));
                }

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Code.Contains(filter.KeyWord) || x.Name.Contains(filter.KeyWord)
                    );
                }
                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }

                if (!string.IsNullOrEmpty(filter.SortColumn))
                {
                    query = query.SortByColumn(filter.SortColumn, filter.IsDescending);
                }

                var data = _mapper.Map<IList<PartnerDto>>(await query.ToListAsync()).ToList();

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
