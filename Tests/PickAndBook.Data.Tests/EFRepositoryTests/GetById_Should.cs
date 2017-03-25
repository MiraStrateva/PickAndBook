using Moq;
using NUnit.Framework;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace PickAndBook.Data.Tests.EFRepositoryTests
{
    [TestFixture]
    public class GetById_Should
    {
        [Test]
        public void ReturnNull_WhenCalledWithNull()
        {
            // Arrange
            var contextMock = new Mock<IPickAndBookDbContext>();
            var categorySetMock = new Mock<IDbSet<Category>>();
            contextMock.Setup(c => c.Set<Category>()).Returns(categorySetMock.Object);

            EFRepository<Category> categoryRepository = new EFRepository<Category>(contextMock.Object);

            // Act
            var result = categoryRepository.GetById(null);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void CallDbSetFindWithExpectedParameter_WhenCalledWithCorrectParameter()
        {
            // Arrange
            var contextMock = new Mock<IPickAndBookDbContext>();
            var categorySetMock = new Mock<IDbSet<Category>>();
            contextMock.Setup(c => c.Set<Category>()).Returns(categorySetMock.Object);

            EFRepository<Category> categoryRepository = new EFRepository<Category>(contextMock.Object);
            var testGuid = Guid.NewGuid();

            // Act
            var result = categoryRepository.GetById(testGuid);

            // Assert
            categorySetMock.Verify(s => s.Find(testGuid), Times.Once());
        }

        [Test]
        public void ReturnResultFromExpectedType_WhenIDFound()
        {
            // Arrange
            var contextMock = new Mock<IPickAndBookDbContext>();
            var categorySetMock = new Mock<IDbSet<Category>>();
            categorySetMock.Setup(c => c.Find(It.IsAny<Guid>())).Returns(new Category()
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = "Name",
                CategoryDescription = "Description",
                CategoryImage = "Image"
            });
            contextMock.Setup(c => c.Set<Category>()).Returns(categorySetMock.Object);

            EFRepository<Category> categoryRepository = new EFRepository<Category>(contextMock.Object);

            // Act
            var result = categoryRepository.GetById(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf<Category>(result);
        }
    }
}
