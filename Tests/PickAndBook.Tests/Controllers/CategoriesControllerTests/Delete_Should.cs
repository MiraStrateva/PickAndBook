using Moq;
using NUnit.Framework;
using PickAndBook.Areas.Admin.Controllers;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.CategoriesControllerTests
{
    [TestFixture]
    public class Delete_Should
    {
        [Test]
        public void ReturnDefaultViewWithExpectedModelType_WhenGetToDelete()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            Guid id = Guid.NewGuid();
            Category category = new Category()
            {
                CategoryId = id,
                CategoryName = "Name",
                CategoryDescription = "Description",
                CategoryImage = "Image"
            };

            categoryRepositoryMock.Setup(c => c.GetById(id)).Returns(category);
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            CategoriesController controller = new CategoriesController(dataMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.Delete(id))
                .ShouldRenderDefaultView()
                .WithModel<Category>();
        }

        [Test]
        public void ReturnDefaultViewWithExpectedModel_WhenGetToDelete()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            Guid id = Guid.NewGuid();
            Category category = new Category()
            {
                CategoryId = id,
                CategoryName = "Name",
                CategoryDescription = "Description",
                CategoryImage = "Image"
            };

            categoryRepositoryMock.Setup(c => c.GetById(id)).Returns(category);
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            CategoriesController controller = new CategoriesController(dataMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.Delete(id))
                .ShouldRenderDefaultView()
                .WithModel(category);
        }
    }
}
