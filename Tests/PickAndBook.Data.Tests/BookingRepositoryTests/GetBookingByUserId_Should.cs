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
    public class GetBookingByUserId_Should
    {
        [Test]
        public void ReturnCorrectResults_WhenUserIdMatches()
        {
            // Arange
            var contextMock = new Mock<IPickAndBookDbContext>();
            String[] userIds = new string[2];
            String testUserId = Guid.NewGuid().ToString();

            userIds[0] = testUserId;
            userIds[1] = Guid.NewGuid().ToString();

            IEnumerable<Booking> bookings = GetBookings(userIds);

            var expectedBookingResultSet = bookings.Where(b => b.UserId.Equals(testUserId)).AsQueryable();

            var bookingSetMock = QueryableDbSetMock.GetQueryableMockDbSet(bookings);
            contextMock.Setup(c => c.Set<Booking>()).Returns(bookingSetMock);

            BookingRepository bookingRepository = new BookingRepository(contextMock.Object);

            // Act
            IQueryable<Booking> resultSet = bookingRepository.GetBookingByUserId(testUserId);

            // Assert
            CollectionAssert.AreEqual(expectedBookingResultSet, resultSet);
        }

        private IEnumerable<Booking> GetBookings(String[] userIds)
        {
            List<Booking> bookings = new List<Booking>();
            int index = 1;

            foreach (String userId in userIds)
            {
                for (int i = index; i < index + 3; i++)
                {
                    bookings.Add(new Booking()
                    {
                        BookingId = Guid.NewGuid(),
                        UserId = userId,
                        ClientName = "Client Name " + i,
                        ClientPhone = "089/558855" + i,
                        CompanyId = Guid.NewGuid(),
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
