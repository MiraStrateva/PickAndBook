using PickAndBook.Data.Contracts;
using PickAndBook.Data.Models;
using System;
using System.Linq;

namespace PickAndBook.Data.Repositories.Contracts
{
    public interface IBookingRepository : IRepository<Booking>
    {
        IQueryable<Booking> GetBookingByUserId(string userId);

        IQueryable<Booking> GetBookingByCompanyId(Guid companyId);
    }
}
