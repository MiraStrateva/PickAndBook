using Moq;
using NUnit.Framework;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories;
using PickAndBook.Data.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PickAndBook.Data.Tests.CategoryRepositoryTests
{
    [TestFixture]
    public class GetAll_Should
    {
        [TestCase(0, 2, 10)]
        [TestCase(1, 2, 3)]
        public void ReturnExpectedObjectCount_WhenCorrectParametersArePassed(int page, int pageSize, int count)
        {
            // Arrange
            var contextMock = new Mock<IPickAndBookDbContext>();
            var categorySetMock = new Mock<IDbSet<Category>>();
            contextMock.Setup(c => c.Set<Category>()).Returns(categorySetMock.Object);

            IEnumerable<Category> categories = GetCategories(count);
            var categoryDbSetMock = QueryableDbSetMock.GetQueryableMockDbSet(categories);
            contextMock.Setup(c => c.Set<Category>()).Returns(categoryDbSetMock);

            CategoryRepository categoryRepository = new CategoryRepository(contextMock.Object);
            int expectedCount = Math.Min(pageSize, count - (page * pageSize));

            // Act
            var resultCategries = categoryRepository.GetAll(page, pageSize);

            // Assert
            Assert.AreEqual(expectedCount, resultCategries.Count());
        }

        [TestCase(0, 2, 10)]
        [TestCase(1, 2, 3)]
        public void ReturnExpectedResult_WhenCorrectParametersArePassed(int page, int pageSize, int count)
        {
            // Arrange
            var contextMock = new Mock<IPickAndBookDbContext>();
            var categorySetMock = new Mock<IDbSet<Category>>();
            contextMock.Setup(c => c.Set<Category>()).Returns(categorySetMock.Object);

            IEnumerable<Category> categories = GetCategories(count);
            var categoryDbSetMock = QueryableDbSetMock.GetQueryableMockDbSet(categories);
            contextMock.Setup(c => c.Set<Category>()).Returns(categoryDbSetMock);

            CategoryRepository categoryRepository = new CategoryRepository(contextMock.Object);
            int toSkip = page * pageSize;
            var expectedCategories = categoryDbSetMock
                       .OrderBy(c => c.CategoryName)
                       .Skip(toSkip)
                       .Take(pageSize);

            // Act
            var resultCategries = categoryRepository.GetAll(page, pageSize);

            // Assert
            CollectionAssert.AreEqual(expectedCategories, resultCategries);
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
