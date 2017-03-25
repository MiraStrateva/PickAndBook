using Moq;
using NUnit.Framework;
using PickAndBook.Controllers;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.HomeControllerTests
{
    [TestFixture]
    public class HomeCategories_Should
    {
        [Test]
        public void ReturnHomeCategoriesPartialView_WhenGetToHomeCategories()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            IEnumerable<Category> categories = GetCategories();
            var expectedCategoryResultSet = categories.AsQueryable();

            categoryRepositoryMock.Setup(m => m.All()).Returns(expectedCategoryResultSet);
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            HomeController controller = new HomeController(dataMock.Object);

            // Act & Assert
            controller.WithCallTo(c => c.HomeCategories())
                .ShouldRenderPartialView("_HomeCategories");
        }

        [Test]
        public void ReturnHomeCategoriesPartialViewWithExpectedModelType_WhenGetToHomeCategories()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            IEnumerable<Category> categories = GetCategories();
            var expectedCategoryResultSet = categories.AsQueryable();

            categoryRepositoryMock.Setup(m => m.All()).Returns(expectedCategoryResultSet);
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            HomeController controller = new HomeController(dataMock.Object);

            // Act & Assert
            controller.WithCallTo(c => c.HomeCategories())
                .ShouldRenderPartialView("_HomeCategories")
                .WithModel<IList<Category>>(); 
        }

        [Test]
        public void ReturnHomeCategoriesPartialViewWithExpectedModel_WhenGetToHomeCategories()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            IEnumerable<Category> categories = GetCategories();
            var expectedCategoryResultSet = categories.AsQueryable();
            var expectedModel = expectedCategoryResultSet.ToList();

            categoryRepositoryMock.Setup(m => m.All()).Returns(expectedCategoryResultSet);
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            HomeController controller = new HomeController(dataMock.Object);

            // Act 
            var result = controller.HomeCategories() as PartialViewResult;

            // Assert
            var actualModel = result.Model as List<Category>;
            CollectionAssert.AreEqual(expectedModel, actualModel);

            //// Act & Assert
            //controller.WithCallTo(c => c.HomeCategories())
            //    .ShouldRenderPartialView("_HomeCategories")
            //    .WithModel(expectedModel);
        }

        private IEnumerable<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();

            for (int i = 1; i < 4; i++)
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
