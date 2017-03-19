using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Base;
using PickAndBook.Data.Repositories.Contracts;
using System;
using System.Linq;

namespace PickAndBook.Data.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(IPickAndBookDbContext context)
            : base(context)
        {
        }

        public IQueryable<Booking> GetBookingByCompanyId(Guid companyId)
        {
            return this.All()
                .Where(b => b.CompanyId == companyId);
        }

        public IQueryable<Booking> GetBookingByUserId(string userId)
        {
            return this.All()
                .Where(b => b.UserId == userId);
        }
    }
}
