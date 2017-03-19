using Moq;
using NUnit.Framework;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickAndBook.Data.Tests.CategoryRepositoryTests
{
    [TestFixture]
    public class GetCategoryNameById_Should
    {
        [Test]
        public void ReturnCorrectCategoryName_WhenValidCategoryIdIsPassed()
        {
            // Arrange
            var contextMock = new Mock<IPickAndBookDbContext>();
            var categorySetMock = new Mock<IDbSet<Category>>();
            contextMock.Setup(c => c.Set<Category>()).Returns(categorySetMock.Object);

            Guid categoryId = Guid.NewGuid();
            string categoryName = "My Category";
            Category category = new Category() { CategoryId = categoryId, CategoryName = categoryName };

            categorySetMock.Setup(c => c.Find(categoryId)).Returns(category);

            CategoryRepository categoryRepository = new CategoryRepository(contextMock.Object);

            // Act
            string resultCategoryName = categoryRepository.GetCategoryNameById(categoryId);

            // Assert
            Assert.AreEqual(categoryName, resultCategoryName);
        }

        [Test]
        public void ReturnEmptyCategoryName_WhenNullCategoryIdIsPassed()
        {
            // Arrange
            var contextMock = new Mock<IPickAndBookDbContext>();
            var categorySetMock = new Mock<IDbSet<Category>>();
            contextMock.Setup(c => c.Set<Category>()).Returns(categorySetMock.Object);

            Guid categoryId = Guid.NewGuid();
            string categoryName = "My Category";
            Category category = new Category() { CategoryId = categoryId, CategoryName = categoryName };

            categorySetMock.Setup(c => c.Find(categoryId)).Returns(category);

            CategoryRepository categoryRepository = new CategoryRepository(contextMock.Object);

            // Act
            string resultCategoryName = categoryRepository.GetCategoryNameById(null);

            // Assert
            Assert.AreEqual(string.Empty, resultCategoryName);
        }

        [Test]
        public void ReturnEmptyCategoryName_WhenCategoryNotFound()
        {
            // Arrange
            var contextMock = new Mock<IPickAndBookDbContext>();
            var categorySetMock = new Mock<IDbSet<Category>>();
            contextMock.Setup(c => c.Set<Category>()).Returns(categorySetMock.Object);

            Guid categoryId = Guid.NewGuid();
            string categoryName = "My Category";
            Category category = new Category() { CategoryId = categoryId, CategoryName = categoryName };

            categorySetMock.Setup(c => c.Find(categoryId)).Returns<Category>(null);

            CategoryRepository categoryRepository = new CategoryRepository(contextMock.Object);

            // Act
            string resultCategoryName = categoryRepository.GetCategoryNameById(Guid.NewGuid());

            // Assert
            Assert.AreEqual(string.Empty, resultCategoryName);
        }
    }
}
