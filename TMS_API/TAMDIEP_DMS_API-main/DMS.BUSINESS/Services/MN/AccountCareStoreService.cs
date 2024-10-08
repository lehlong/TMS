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
    public interface IAccountCareStoreService : IGenericService<TblMnAccountCareStore, AccountCareStoreDto>
    {
        Task<PagedResponseDto> Search(AccountCareStoreFilter filter);
        Task<List<AccountCareStoreDto>> GetAll(AccountCareStoreFilter filter);
        Task<List<AccountCareStoreDto>> GetByPartnerId(AccountCareStoreFilter filter);
        Task<AccountCareStoreDto> GetById(int id);
    }

    public class AccountCareStoreService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMnAccountCareStore, AccountCareStoreDto>(dbContext, mapper), IAccountCareStoreService
    {
        public async Task<List<AccountCareStoreDto>> GetAll(AccountCareStoreFilter filter)
        {
            try
            {
                var query = _dbContext.TblMnAccountCareStores.Include(x => x.Partner).Include(x => x.AccountPlanVisit)
                    .Where(x => filter.Year == null || x.AccountPlanVisit.Year == filter.Year)
                    .Where(x => filter.Month == null || x.AccountPlanVisit.Month == filter.Month)
                    .Where(x => string.IsNullOrEmpty(filter.UserName) || x.AccountPlanVisit.UserName == filter.UserName)
                    .Where(x => filter.PartnerId == null || x.PartnerId == filter.PartnerId);


                if (filter.FromDate.HasValue)
                {
                    query = query.Where(x => x.ActionDate >= filter.FromDate.Value);
                }

                if (filter.ToDate.HasValue)
                {
                    var endDate = filter.ToDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(x => x.ActionDate <= endDate);
                }

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Contents.Contains(filter.KeyWord)
                    );
                }

                if (filter.Fields != null && filter.Fields.Count != 0)
                {
                    query = query.SelectFields(filter.Fields);
                }

                if (!string.IsNullOrEmpty(filter.SortColumn))
                {
                    query = query.SortByColumn(filter.SortColumn, filter.IsDescending);
                }

                var result = await query.ToListAsync();

                return _mapper.Map<List<AccountCareStoreDto>>(result);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<AccountCareStoreDto> GetById(int id)
        {
            var found = await _dbContext.TblMnAccountCareStores.Where(x => x.Id == id).Include(x => x.Partner).Include(x => x.AccountPlanVisit).FirstOrDefaultAsync();
            return _mapper.Map<AccountCareStoreDto>(found);
        }

        public async Task<List<AccountCareStoreDto>> GetByPartnerId(AccountCareStoreFilter filter)
        {
            try
            {
                var query = _dbContext.TblMnAccountCareStores.Where(x => x.PartnerId == filter.PartnerId)
                    .Include(x => x.Partner).Include(x => x.AccountPlanVisit)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Contents.Contains(filter.KeyWord) ||
                        x.AccountPlanVisit.PlanName.Contains(filter.KeyWord)
                    );
                }

                if (!string.IsNullOrEmpty(filter.SortColumn))
                {
                    query = query.SortByColumn(filter.SortColumn, filter.IsDescending);
                }

                var result = await query.ToListAsync();

                return _mapper.Map<List<AccountCareStoreDto>>(result);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<PagedResponseDto> Search(AccountCareStoreFilter filter)
        {
            try
            {
                var query = _dbContext.TblMnAccountCareStores.Include(x => x.Partner).Include(x => x.AccountPlanVisit)
                    .Where(x => filter.Year == null || x.AccountPlanVisit.Year == filter.Year)
                    .Where(x => filter.Month == null || x.AccountPlanVisit.Month == filter.Month)
                    .Where(x => string.IsNullOrEmpty(filter.UserName) || x.AccountPlanVisit.UserName == filter.UserName)
                    .Where(x => filter.PartnerId == null || x.PartnerId == filter.PartnerId);

                if (filter.FromDate.HasValue)
                {
                    query = query.Where(x => x.ActionDate >= filter.FromDate.Value);
                }

                if (filter.ToDate.HasValue)
                {
                    var endDate = filter.ToDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(x => x.ActionDate <= endDate);
                }

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Contents.Contains(filter.KeyWord)
                    );
                }

                if (filter.Fields != null && filter.Fields.Count != 0)
                {
                    query = query.SelectFields(filter.Fields);
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
    }
}
