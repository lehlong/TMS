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
    public interface IContractService : IGenericService<TblMnContract, ContractDto>
    {
        Task<List<ContractDto>> GetAll(ContractGetAllFilter filter);
    }

    public class ContractService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMnContract, ContractDto>(dbContext, mapper), IContractService
    {
        public async Task<List<ContractDto>> GetAll(ContractGetAllFilter filter)
        {
            try
            {
                var query = _dbContext.TblMnContract
                    .Include(x => x.PartnerContracts)
                    .Where(x => filter.PartnerId == null || x.PartnerContracts.Any(y => y.PartnerId == filter.PartnerId))
                    .AsQueryable();

                return await base.GetAllMd(query, filter);
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
