using Moq;
using NUnit.Framework;
using PickAndBook.Areas.Admin.Controllers;
using PickAndBook.Common;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using PickAndBook.Tests.Helpers;
using System;
using System.Web.Mvc;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.CategoriesControllerTests
{
    [TestFixture]
    public class DeleteCategory_Should
    {
        [TestCase(typeof(HttpPostAttribute))]
        [TestCase(typeof(ValidateAntiForgeryTokenAttribute))]
        public void Have_RequiredAttributes(Type attrType)
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            CategoriesController controller = new CategoriesController(dataMock.Object);

            // Act
            var hasAttribute = AttributeTester.MethodHasAttribute(() => controller.DeleteCategory(It.IsAny<Guid>()), attrType);

            // Assert
            Assert.IsTrue(hasAttribute);
        }

        [Test]
        public void RedirectToIndex_WhenCategoryIsDeleted()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            Category category = new Category()
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = "Name 1",
                CategoryDescription = "Name 2",
                CategoryImage = "Image 1"
            };

            categoryRepositoryMock.Setup(c => c.GetById(It.IsAny<Guid>())).Returns(category);
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            CategoriesController controller = new CategoriesController(dataMock.Object);

            // Act & Assert
            controller.WithCallTo(c => c.DeleteCategory(Guid.NewGuid()))
                .ShouldRedirectTo(c => c.Index(0, Constants.DefaultPageSize));
        }
    }
}
