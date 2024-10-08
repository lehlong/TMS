using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.MN;
using DMS.BUSINESS.Filter.MN;
using DMS.CORE;
using DMS.CORE.Entities.MN;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.MN
{
    public interface IAccountPlanVisitService : IGenericService<TblMnAccountPlanVisit, AccountPlanVisitDto>
    {
        Task<PagedResponseDto> Search(AccountPlanVisitFilter filter);
        Task<AccountPlanVisitDto> Add(AccountPlanVisitCreateDto param);
        Task Update(AccountPlanVisitUpdateDto param);
        Task<AccountPlanVisitDto> GetByUserName(AccountPlanVisitUserFilter filter);
        Task<List<AccountPlanVisitDto>> GetAll(AccountPlanVisitFilter filter);
        Task DeleteList(int[] listIds);
    }

    public class AccountPlanVisitService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMnAccountPlanVisit, AccountPlanVisitDto>(dbContext, mapper), IAccountPlanVisitService
    {
        public async Task<AccountPlanVisitDto> Add(AccountPlanVisitCreateDto param)
        {
            try
            {
                var foundsASO = await _dbContext.TblMnAccountSaleOffices.Where(x => x.UserName == param.UserName).ToListAsync();
                if (foundsASO == null || foundsASO.Count == 0)
                {
                    this.Status = false;
                    this.MessageObject.Code = "2000";
                    return null;
                }
                var partnerIds = foundsASO.Select(x => x.PartnerId).ToList();

                var modelDto = _mapper.Map<AccountPlanVisitDto>(param);
                var result = await base.Add(modelDto);

                foreach (var partnerId in param.PartnerIds)
                {
                    var newAPVS = new TblMnAccountPlanVisitStore()
                    {
                        PlanId = result.Id,
                        PartnerId = partnerId
                    };
                    await _dbContext.TblMnAccountPlanVisitStores.AddAsync(newAPVS);
                    await _dbContext.SaveChangesAsync();
                }
                return result;
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return null;
            }
        }

        public async Task DeleteList(int[] listIds)
        {
            try
            {
                var founds = await _dbContext.TblMnAccountPlanVisits.Where(x => listIds.Contains(x.Id)).ToListAsync();
                _dbContext.TblMnAccountPlanVisits.RemoveRange(founds);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
            }
        }

        public async Task<List<AccountPlanVisitDto>> GetAll(AccountPlanVisitFilter filter)
        {
            try
            {
                var query = _dbContext.TblMnAccountPlanVisits.Include(x => x.Account)
                    .Include(x => x.AccountPlanVisitStores).ThenInclude(y => y.Partner)
                    .AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.PlanName.Contains(filter.KeyWord)
                    );
                }

                if (filter.Year.HasValue)
                {
                    query = query.Where(x => x.Year == filter.Year);
                }

                if (filter.Month.HasValue)
                {
                    query = query.Where(x => x.Month == filter.Month);
                }

                if (!string.IsNullOrEmpty(filter.AccountSaleOffice))
                {
                    query = query.Where(x => x.UserName == filter.AccountSaleOffice);
                }

                if (!string.IsNullOrEmpty(filter.SortColumn))
                {
                    query = query.SortByColumn(filter.SortColumn, filter.IsDescending);
                }
                var result = await query.ToListAsync();
                return _mapper.Map<List<AccountPlanVisitDto>>(result);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<AccountPlanVisitDto> GetByUserName(AccountPlanVisitUserFilter filter)
        {
            var query = _dbContext.TblMnAccountPlanVisits.Where(x => x.UserName == filter.UserName)
                                    .Include(x => x.AccountPlanVisitStores).ThenInclude(y => y.Partner).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.KeyWord))
            {
                query = query.Where(x =>
                    x.PlanName.Contains(filter.KeyWord)
                );
            }

            if (filter.Year.HasValue)
            {
                query = query.Where(x => x.Year == filter.Year);
            }

            if (filter.Month.HasValue)
            {
                query = query.Where(x => x.Month == filter.Month);
            }

            var result = await query.FirstOrDefaultAsync();
            return _mapper.Map<AccountPlanVisitDto>(result);
        }

        public async Task<PagedResponseDto> Search(AccountPlanVisitFilter filter)
        {
            try
            {
                var query = _dbContext.TblMnAccountPlanVisits.Include(x => x.Account)
                    .Include(x => x.AccountPlanVisitStores).ThenInclude(y => y.Partner)
                    .AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.PlanName.Contains(filter.KeyWord)
                    );
                }

                if (filter.Year.HasValue)
                {
                    query = query.Where(x => x.Year == filter.Year);
                }

                if (filter.Month.HasValue)
                {
                    query = query.Where(x => x.Month == filter.Month);
                }

                if (!string.IsNullOrEmpty(filter.AccountSaleOffice))
                {
                    query = query.Where(x => x.UserName == filter.AccountSaleOffice);
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

        public async Task Update(AccountPlanVisitUpdateDto param)
        {
            try
            {
                var foundAPV = await _dbContext.TblMnAccountPlanVisits.Include(x => x.AccountPlanVisitStores).Where(x => x.Id == param.Id).FirstOrDefaultAsync();

                if (foundAPV == null)
                {
                    return;
                }

                var modelDto = _mapper.Map<AccountPlanVisitDto>(param);
                await base.Update(modelDto);

                var foundsASO = await _dbContext.TblMnAccountSaleOffices.Where(x => x.UserName == param.UserName).ToListAsync();
                if (foundsASO == null || foundsASO.Count == 0)
                {
                    this.Status = false;
                    this.MessageObject.Code = "2000";
                    return;
                }
                var partnerIds = foundsASO.Select(x => x.PartnerId).ToList();

                var partnerIdsOld = foundAPV.AccountPlanVisitStores.Select(x => x.PartnerId).ToList();

                var partnerIdsRemove = partnerIdsOld.Except(param.PartnerIds);
                foreach (var item in partnerIdsRemove)
                {
                    var itemRemove = await _dbContext.TblMnAccountPlanVisitStores.Where(x => x.PartnerId == item).FirstOrDefaultAsync();
                    _dbContext.TblMnAccountPlanVisitStores.Remove(itemRemove);
                    await _dbContext.SaveChangesAsync();
                }

                var partnerIdsAdd = param.PartnerIds.Except(partnerIdsOld);

                foreach (var partnerId in partnerIdsAdd)
                {
                    var newAPVS = new TblMnAccountPlanVisitStore()
                    {
                        PlanId = param.Id,
                        PartnerId = partnerId
                    };
                    await _dbContext.TblMnAccountPlanVisitStores.AddAsync(newAPVS);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
            }
        }
    }
}
