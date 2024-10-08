using AutoMapper;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.MN;
using DMS.CORE;
using DMS.CORE.Entities.MN;

namespace DMS.BUSINESS.Services.MN
{
    public interface IAccountPlanVisitStoreService : IGenericService<TblMnAccountPlanVisitStore, AccountPlanVisitStoreDto>
    {
    }

    public class AccountPlanVisitStoreService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMnAccountPlanVisitStore, AccountPlanVisitStoreDto>(dbContext, mapper), IAccountPlanVisitStoreService
    {
    }
}
