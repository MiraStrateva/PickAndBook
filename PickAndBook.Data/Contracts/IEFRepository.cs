using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PickAndBook.Data.Contracts
{
    public interface IEFRepository<T>
            where T : class
    {
        IQueryable<T> All();

        T GetById(Guid? id);

        void Add(T entity);

        // Use only in transaction scope
        void Add(IEnumerable<T> entities);

        void Update(T entity);

        int Update(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T>> updateExpression);

        void Delete(T entity);

        void Delete(Guid id);

        int Delete(Expression<Func<T, bool>> filterExpression);

        void Detach(T entity);
    }
}
