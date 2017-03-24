using System;
using System.Collections.Generic;
using PickAndBook.Data.Contracts;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using PickAndBook.Data.Repositories;
using PickAndBook.Data.Repositories.Base;

namespace PickAndBook.Data
{
    public class PickAndBookData : IPickAndBookData
    {
        private readonly IPickAndBookDbContext context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public PickAndBookData(IPickAndBookDbContext context)
        {
            this.context = context;
        }

        public IBookingRepository Bookings => (BookingRepository)this.GetRepository<Booking>();

        public ICategoryRepository Categories => (CategoryRepository)this.GetRepository<Category>();

        public ICompanyRepository Companies => (CompanyRepository)this.GetRepository<Company>();

        public IEFRepository<Workingtime> Workingtimes => (EFRepository<Workingtime>)this.GetRepository<Workingtime>();

        public IPickAndBookDbContext Context => this.context;

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of objects written to the underlying database.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the context has been disposed.</exception>
        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context?.Dispose();
            }
        }

        private IEFRepository<T> GetRepository<T>()
            where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(EFRepository<T>);

                if (typeof(T).IsAssignableFrom(typeof(Category)))
                {
                    type = typeof(CategoryRepository);
                }

                if (typeof(T).IsAssignableFrom(typeof(Company)))
                {
                    type = typeof(CompanyRepository);
                }

                if (typeof(T).IsAssignableFrom(typeof(Booking)))
                {
                    type = typeof(BookingRepository);
                }

                if (typeof(T).IsAssignableFrom(typeof(Workingtime)))
                {
                    type = typeof(EFRepository<Workingtime>);
                }
                
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IEFRepository<T>)this.repositories[typeof(T)];
        }
    }
}
