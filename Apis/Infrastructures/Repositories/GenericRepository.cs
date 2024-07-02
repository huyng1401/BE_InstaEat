using Application.Interfaces;
using Application.Commons;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repositories;
using System.Linq.Expressions;

namespace Infrastructures.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            var result = await _dbSet.FindAsync(id);
            if (result == null)
            {
                throw new Exception("Not found");
            }
            return result;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }
        public async Task<List<TEntity>> GetAllNotDeletedAsync()
        {
            return await _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false).ToListAsync();
        }
        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }


        public async Task<Pagination<TEntity>> PaginateList(List<TEntity> list, int pageIndex = 0, int pageSize = 10)
        {
            var itemCount = list.Count;
            var items = list.Skip(pageIndex * pageSize)
                            .Take(pageSize)
                            .ToList();

            var result = new Pagination<TEntity>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<TEntity>> ToPagination(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbSet.CountAsync();
            var items = await _dbSet.Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            var result = new Pagination<TEntity>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }
        public async Task<Pagination<TEntity>> PaginateFiltered(Expression<Func<TEntity, bool>> filter, int pageIndex = 0, int pageSize = 10)
        {
            var filteredList = await _dbSet.Where(filter).ToListAsync();
            return await PaginateList(filteredList, pageIndex, pageSize);
        }

        public void UpdateRange(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

    }
}
