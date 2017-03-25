using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Base;
using PickAndBook.Data.Repositories.Contracts;
using System;
using System.Linq;

namespace PickAndBook.Data.Repositories
{
    public class BookingRepository : EFRepository<Booking>, IBookingRepository
    {
        public BookingRepository(IPickAndBookDbContext context)
            : base(context)
        {
        }

        public IQueryable<Booking> GetBookingByCompanyId(Guid? companyId)
        {
            return companyId.HasValue ? 
                this.All().Where(b => b.CompanyId == companyId) :
                null;
        }

        public IQueryable<Booking> GetBookingByUserId(string userId)
        {
            return this.All()
                .Where(b => b.UserId == userId);
        }
    }
}
