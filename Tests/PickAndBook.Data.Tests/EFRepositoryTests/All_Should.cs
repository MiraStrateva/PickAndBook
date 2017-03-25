using Moq;
using NUnit.Framework;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Base;
using PickAndBook.Data.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickAndBook.Data.Tests.EFRepositoryTests
{
    [TestFixture]
    public class All_Should
    {
        [Test]
        public void ReturnIQuerableFromExpectedType_WhenCalled()
        {
            // Arrange
            var contextMock = new Mock<IPickAndBookDbContext>();
            var categorySetMock = new Mock<IDbSet<Category>>();
            contextMock.Setup(c => c.Set<Category>()).Returns(categorySetMock.Object);

            EFRepository<Category> categoryRepository = new EFRepository<Category>(contextMock.Object);

            // Act
            var result = categoryRepository.All();

            // Assert
            Assert.IsInstanceOf<IQueryable<Category>>(result);
        }

        [Test]
        public void ReturnExpectedResult_WhenCalled()
        {
            // Arrange
            var contextMock = new Mock<IPickAndBookDbContext>();

            IEnumerable<Category> categories = GetCategories(5);
            var categorySetMock = QueryableDbSetMock.GetQueryableMockDbSet(categories);

            contextMock.Setup(c => c.Set<Category>()).Returns(categorySetMock);

            EFRepository<Category> categoryRepository = new EFRepository<Category>(contextMock.Object);

            // Act
            var result = categoryRepository.All();

            // Assert
            Assert.AreEqual(categorySetMock, result);
        }

        private IEnumerable<Category> GetCategories(int count)
        {
            List<Category> categories = new List<Category>();

            for (int i = 1; i <= count; i++)
            {
                categories.Add(new Category()
                {
                    CategoryId = Guid.NewGuid(),
                    CategoryName = "Name " + i,
                    CategoryDescription = "Description " + i
                });
            }

            return categories;
        }
    }
}
