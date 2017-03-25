using PickAndBook.Data.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace PickAndBook.Data
{
    public interface IPickAndBookDbContext : IDisposable
    { 
        IDbSet<Booking> Bookings { get; }

        IDbSet<Category> Categories { get; }

        IDbSet<Company> Companies { get; }

        IDbSet<Workingtime> Workingtimes { get; }

        DbContext DbContext { get; }

        int SaveChanges();

        void RefreshAll();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;

        IDbSet<T> Set<T>()
            where T : class;
    }
}
