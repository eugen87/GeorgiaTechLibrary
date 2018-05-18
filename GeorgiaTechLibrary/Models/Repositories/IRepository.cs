using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GeorgiaTechLibrary.Models.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Add(params T[] entities);
        void Add(IEnumerable<T> entities);

        void Delete(T entity);
        void Delete(object id);
        void Delete(params T[] entities);
        void Delete(IEnumerable<T> entities);

        void Update(T entity);
        void Update(params T[] entities);
        void Update(IEnumerable<T> entities);

        IEnumerable<T> Get();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
    }
}
