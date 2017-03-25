using Moq;
using NUnit.Framework;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories;
using PickAndBook.Data.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PickAndBook.Data.Tests.BookingRepositoryTests
{
    [TestFixture]
    public class GetBookingByCompanyId_Should
    {
        [Test]
        public void ReturnNull_WhenNullCompanyIdPassed()
        {
            // Arrange
            var contextMock = new Mock<IPickAndBookDbContext>();
            BookingRepository bookingRepository = new BookingRepository(contextMock.Object);

            // Act
            var result = bookingRepository.GetBookingByCompanyId(null);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void ReturnCorrectResults_WhenCompanyIdMatches()
        {
            // Arange
            var contextMock = new Mock<IPickAndBookDbContext>();
            Guid[] companyIds = new Guid[2];
            Guid testCompanyId = Guid.NewGuid();

            companyIds[0] = testCompanyId;
            companyIds[1] = Guid.NewGuid();

            IEnumerable<Booking> bookings = GetBookings(companyIds);

            var expectedBookingResultSet = bookings.Where(b => b.CompanyId.Equals(testCompanyId)).AsQueryable();

            var bookingSetMock = QueryableDbSetMock.GetQueryableMockDbSet(bookings);
            contextMock.Setup(c => c.Set<Booking>()).Returns(bookingSetMock);

            BookingRepository bookingRepository = new BookingRepository(contextMock.Object);

            // Act
            IQueryable<Booking> resultSet = bookingRepository.GetBookingByCompanyId(testCompanyId);

            // Assert
            CollectionAssert.AreEqual(expectedBookingResultSet, resultSet);
        }

        private IEnumerable<Booking> GetBookings(Guid[] companyIds)
        {
            List<Booking> bookings = new List<Booking>();
            int index = 1;

            foreach (Guid companyId in companyIds)
            {
                for (int i = index; i < index + 3; i++)
                {
                    bookings.Add(new Booking()
                    {
                        BookingId = Guid.NewGuid(),
                        UserId = Guid.NewGuid().ToString(),
                        ClientName = "Client Name " + i,
                        ClientPhone = "089/558855" + i,
                        CompanyId = companyId,
                        StartFrom = DateTime.Now,
                        EndAt = DateTime.Now
                    });
                }
                index += 3;
            }

            return bookings;
        }

    }
}
