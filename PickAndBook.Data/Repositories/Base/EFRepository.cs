using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Extensions;
using PickAndBook.Data.Contracts;
using System.Data.Entity.Migrations;
using Bytes2you.Validation;

namespace PickAndBook.Data.Repositories.Base
{
    public class EFRepository<T> : IEFRepository<T>
        where T : class
    {
        public EFRepository(IPickAndBookDbContext context)
        {
            Guard.WhenArgument(context, nameof(context)).IsNull().Throw();

            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }

        protected IDbSet<T> DbSet { get; set; }

        protected IPickAndBookDbContext Context { get; set; }

        public virtual IQueryable<T> All()
        {
            this.Context.RefreshAll();
            return this.DbSet.AsQueryable();
        }

        public virtual T GetById(Guid? id)
        {
            return this.DbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.DbSet.Add(entity);
            }
        }

        public void Add(IEnumerable<T> entities)
        {
            this.Context.DbContext.BulkInsert(entities);
        }

        public virtual void Update(T entity)
        {
            this.DbSet.AddOrUpdate(entity);
        }

        public virtual int Update(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T>> updateExpression)
        {
            return this.DbSet.Where(filterExpression).Update(updateExpression);
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.DbSet.Attach(entity);
                this.DbSet.Remove(entity);
            }
        }

        public virtual void Delete(Guid id)
        {
            var entity = this.GetById(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public virtual int Delete(Expression<Func<T, bool>> filterExpression)
        {
            return this.DbSet.Where(filterExpression).Delete();
        }

        public virtual void Detach(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);

            entry.State = EntityState.Detached;
        }
    }
}
