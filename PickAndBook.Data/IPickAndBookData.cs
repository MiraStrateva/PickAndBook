using PickAndBook.Data.Contracts;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using System;

namespace PickAndBook.Data
{
    public interface IPickAndBookData : IDisposable
    {
        ICategoryRepository Categories { get; }

        ICompanyRepository Companies { get; }

        IBookingRepository Bookings { get; }

        IEFRepository<Workingtime> Workingtimes { get; }

        IPickAndBookDbContext Context { get; }

        int SaveChanges();
    }
}
