using Microsoft.AspNet.Identity.EntityFramework;
using PickAndBook.Data.Contracts;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using System;

namespace PickAndBook.Data
{
    public interface IPickAndBookData : IDisposable
    {
        IBookingRepository Bookings { get; }

        ICategoryRepository Categories { get; }

        ICompanyRepository Companies { get; }

        IRepository<Workingtime> Workingtimes { get; }

        IRepository<IdentityRole> Roles { get; }

        IRepository<IdentityUser> Users { get; }

        IPickAndBookDbContext Context { get; }

        int SaveChanges();
    }
}
