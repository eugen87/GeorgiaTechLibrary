using GeorgiaTechLibrary.Models.Members;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace GeorgiaTechLibraryAPI.Models.Repositories
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public RepositoryAsync(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public Task AddAsync(T entity)
        {
            AddAsync(entity, new CancellationToken());
            return _dbContext.SaveChangesAsync();

        }

        public Task AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            _dbSet.AddAsync(entity, cancellationToken);
            return _dbContext.SaveChangesAsync();
        }

        public Task AddAsync(params T[] entities)
        {
            _dbSet.AddRangeAsync(entities);
            return _dbContext.SaveChangesAsync();
        }


        public Task AddAsync(IEnumerable<T> entities,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            _dbSet.AddRangeAsync(entities, cancellationToken);
            return _dbContext.SaveChangesAsync();
        }



        public async Task DeleteAsync(T entity)
        {
            var existing = await _dbSet.FindAsync(entity);
            if (existing != null) _dbSet.Remove(existing);
        }

        public async Task DeleteAsync(object id)
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var key = _dbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);
            if (property != null)
            {
                var entity = Activator.CreateInstance<T>();
                property.SetValue(entity, id);
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null) await DeleteAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(params T[] entities)
        {
            _dbSet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }


        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(params T[] entities)
        {
            _dbSet.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsEnumerable();
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task<IEnumerable<T>> GetAsync() => _dbSet.AsEnumerable();
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

    }
}
