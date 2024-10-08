using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Filter.MD;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.MD
{
    public interface IAreaService : IGenericService<TblMdArea, AreaDto>
    {
     
    }
    public class AreaService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdArea, AreaDto>(dbContext, mapper), IAreaService
    {
       
        

        public override async Task Update(IDto dto)
        {
            if(dto is AreaCreateUpdateDto model)
            {
                var currentObj = await _dbContext.TblMdArea
                    //.Include(x => x.ItemReferences)
                    .FirstOrDefaultAsync(x=>x.Id == model.Id);
                 await base.UpdateWithListInside(dto,currentObj);
            }
        }
    }
}
