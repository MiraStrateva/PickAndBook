using Moq;
using NUnit.Framework;
using PickAndBook.Areas.Admin.Controllers;
using PickAndBook.Common;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using PickAndBook.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers
{
    [TestFixture]
    public class CategoriesControllerTest
    {
        [TestCase(0, 2)]
        [TestCase(1, 3)]
        public void CallCategoryGetAllOnceWithCorrectParameters_WhenGetToIndex(int page, int expectedPageSize)
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            CategoriesController controller = new CategoriesController(dataMock.Object);

            // Act
            controller.Index(page, expectedPageSize);

            // Assert
            categoryRepositoryMock.Verify(s => s.GetAll(page, expectedPageSize), Times.Once()); 
        }

        public void CallCategoryGetAllOnceWithDefaultParameters_WhenGetToIndexWithNoParameters()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            CategoriesController controller = new CategoriesController(dataMock.Object);

            // Act
            controller.Index();

            // Assert
            categoryRepositoryMock.Verify(s => s.GetAll(0, Constants.DefaultPageSize), Times.Once());
        }

        [TestCase(0, 2, 7)]
        [TestCase(1, 3, 8)]
        public void PassExpectedModelToView_WhenGetToIndex(int page, int pageSize, int count)
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var categoriesAll = GetCategories(count);
            var categories = categoriesAll
                .OrderBy(c => c.CategoryName)
                .Skip(page * pageSize)
                .Take(pageSize);

            categoryRepositoryMock.Setup(c => c.GetAll(page, pageSize)).Returns(categories.AsQueryable());
            categoryRepositoryMock.Setup(c => c.All()).Returns(categoriesAll.AsQueryable());
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            CategoriesController controller = new CategoriesController(dataMock.Object);

            var expectedViewModel = new PageableViewModel<Category>()
            {
                Items = categories.ToList(),
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = count
            };

            // Act 
            var result = controller.Index(page, pageSize) as ViewResult;

            // Assert
            var actualModel = result.Model as PageableViewModel<Category>;
            Assert.AreEqual(expectedViewModel.CurrentPage, actualModel.CurrentPage);
            Assert.AreEqual(expectedViewModel.PageSize, actualModel.PageSize);
            Assert.AreEqual(expectedViewModel.TotalCount, actualModel.TotalCount);
            CollectionAssert.AreEqual(expectedViewModel.Items, actualModel.Items);
        }

        [TestCase(0, 2, 7)]
        [TestCase(1, 3, 8)]
        public void RunDefaultView_WhenGetToIndex(int page, int pageSize, int count)
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var categoriesAll = GetCategories(count);
            var categories = categoriesAll
                .OrderBy(c => c.CategoryName)
                .Skip(page * pageSize)
                .Take(pageSize);

            categoryRepositoryMock.Setup(c => c.GetAll(page, pageSize)).Returns(categories.AsQueryable());
            categoryRepositoryMock.Setup(c => c.All()).Returns(categoriesAll.AsQueryable());
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            CategoriesController controller = new CategoriesController(dataMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.Index(page, pageSize))
                .ShouldRenderDefaultView();
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
