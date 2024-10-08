using Common;

namespace XHTD.BUSINESS.Common
{
    public interface IXHTDGenericService<TEntity, TDto> : IBaseService where TEntity : class where TDto : class
    {
        Task<PagedResponseDto> Search(BaseFilter filter);
        Task<IList<TDto>> GetAll();
        Task<IList<TDto>> GetAllActive();
        Task<TDto> GetById(object id);
        Task<TDto> Add(IDto dto);
        Task Update(IDto dto);
        Task Delete(object code);
        Task<PagedResponseDto> Paging(IQueryable<TEntity> query, BaseFilter filter);
    }
}
