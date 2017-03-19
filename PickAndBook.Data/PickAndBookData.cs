using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using PickAndBook.Data.Contracts;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using BookingSystem.Data;
using PickAndBook.Data.Repositories.Base;
using PickAndBook.Data.Repositories;

namespace PickAndBook.Data
{
    public class PickAndBookData : IPickAndBookData
    {
        private readonly IPickAndBookDbContext context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public PickAndBookData()
            : this(new PickAndBookDbContext())
        {
        }

        public PickAndBookData(IPickAndBookDbContext context)
        {
            this.context = context;
        }


        public IBookingRepository Bookings => (BookingRepository)this.GetRepository<Booking>();


        public ICategoryRepository Categories
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICompanyRepository Companies
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPickAndBookDbContext Context
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IRepository<IdentityRole> Roles
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IRepository<IdentityUser> Users
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IRepository<Workingtime> Workingtimes
        {
            get
            {
                throw new NotImplementedException();
            }
        }


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

        private IRepository<T> GetRepository<T>()
            where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                if (typeof(T).IsAssignableFrom(typeof(Booking)))
                {
                    type = typeof(BookingRepository);
                }

                if (typeof(T).IsAssignableFrom(typeof(Category)))
                {
                    type = typeof(CategoryRepository);
                }

                if (typeof(T).IsAssignableFrom(typeof(Company)))
                {
                    type = typeof(CompanyRepository);
                }

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}
