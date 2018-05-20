using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace GeorgiaTechLibraryAPI.Models.Repositories
{
    public interface IRepositoryAsync<T> where T : class
    {

        Task AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        Task AddAsync(params T[] entities);

        Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));


        Task UpdateAsync(T entity);
        Task UpdateAsync(params T[] entities);
        Task UpdateAsync(IEnumerable<T> entities);

        Task DeleteAsync(T entity);
        Task DeleteAsync(object id);
        Task DeleteAsync(params T[] entities);
        Task DeleteAsync(IEnumerable<T> entities);

        Task<IEnumerable<T>> GetAsync();
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
    }
}
