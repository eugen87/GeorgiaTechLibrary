using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(T entity)
        {
            _unitOfWork.Context.Set<T>().Add(entity);
        }

        public void Add(params T[] entities)
        {
            throw new NotImplementedException();
        }

        public void Add(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(entity);
            if (existing != null) _unitOfWork.Context.Set<T>().Remove(existing);
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(params T[] entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get()
        {
            return _unitOfWork.Context.Set<T>().AsEnumerable<T>();
        }

        public IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _unitOfWork.Context.Set<T>().Where(predicate).AsEnumerable<T>();
        }

        public void Update(T entity)
        {
            _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
            _unitOfWork.Context.Set<T>().Attach(entity);
        }

        public void Update(params T[] entities)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}
