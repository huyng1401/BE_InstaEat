using Application.Commons;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        System.Threading.Tasks.Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        //void SoftRemove(TEntity entity);
        System.Threading.Tasks.Task AddRangeAsync(List<TEntity> entities);
        //void SoftRemoveRange(List<TEntity> entities);
        Task<Pagination<TEntity>> PaginateList(List<TEntity> list, int pageIndex = 0, int pageSize = 10);
        Task<Pagination<TEntity>> ToPagination(int pageNumber = 0, int pageSize = 10);
        Task<List<TEntity>> GetAllNotDeletedAsync();
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        Task<Pagination<TEntity>> PaginateFiltered(Expression<Func<TEntity, bool>> filter, int pageIndex = 0, int pageSize = 10);
    }
}
