using AutoMapper;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Common.Enum;
using DMS.BUSINESS.Dtos.AD;
using DMS.BUSINESS.Dtos.MN;
using DMS.CORE;
using DMS.CORE.Entities.MN;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.MN
{
    public interface IAccountSaleOfficeService : IGenericService<TblMnAccountSaleOffice, AccountSaleOfficeDto>
    {
        Task Update(AccountSaleOfficeUpdateDto param);
        Task Delete(AccountSaleOfficeUpdateDto param);
        Task<IList<AccountDto>> GetAllDistinct();
    }

    public class AccountSaleOfficeService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMnAccountSaleOffice, AccountSaleOfficeDto>(dbContext, mapper), IAccountSaleOfficeService
    {
        public async Task Update(AccountSaleOfficeUpdateDto param)
        {
            try
            {
                var founds = await _dbContext.TblMnAccountSaleOffices.Where(x => x.UserName == param.UserName).ToListAsync();
                if (founds == null || founds.Count == 0)
                {
                    await this.AddNewSaleOffices(param.UserName, param.PartnerIds);
                }
                else
                {
                    var partnerIdsOld = founds.Select(x => x.PartnerId).ToList();
                    var partnerIdsAdd = param.PartnerIds.Except(partnerIdsOld);
                    await _dbContext.SaveChangesAsync();

                    await this.AddNewSaleOffices(param.UserName, partnerIdsAdd);
                }
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
            }
        }

        public async Task Delete(AccountSaleOfficeUpdateDto param)
        {
            try
            {
                var founds = await _dbContext.TblMnAccountSaleOffices.Where(x => x.UserName == param.UserName
                                                                    && param.PartnerIds.Contains(x.PartnerId)).ToListAsync();
                if (founds == null || founds.Count == 0)
                {
                    return;
                }
                _dbContext.TblMnAccountSaleOffices.RemoveRange(founds);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
            }
        }

        private async Task AddNewSaleOffices(string userName, IEnumerable<int> partnerIds)
        {
            foreach (var partnerId in partnerIds)
            {
                var newItem = new TblMnAccountSaleOffice
                {
                    UserName = userName,
                    PartnerId = partnerId,
                };
                await _dbContext.TblMnAccountSaleOffices.AddAsync(newItem);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IList<AccountDto>> GetAllDistinct()
        {
            try
            {
                var founds = await _dbContext.TblAdAccount.Where(x => x.AccountType == AccountType.NM_TV.ToString()).ToListAsync();

                return _mapper.Map<IList<AccountDto>>(founds);
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
