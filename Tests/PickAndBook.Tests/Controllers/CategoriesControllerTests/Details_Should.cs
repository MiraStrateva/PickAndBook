﻿using Moq;
using NUnit.Framework;
using PickAndBook.Areas.Admin.Controllers;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using System;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.CategoriesControllerTests
{
    [TestFixture]
    public class Details_Should
    {
        [Test]
        public void ReturnDefaultViewWithExpectedModelType_WhenGetToDetails()
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
            controller.WithCallTo(c => c.Details(id))
                .ShouldRenderDefaultView()
                .WithModel<Category>();
        }


        [Test]
        public void ReturnDefaultViewWithExpectedModel_WhenGetToDetails()
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
            controller.WithCallTo(c => c.Details(id))
                .ShouldRenderDefaultView()
                .WithModel(category);
        }
    }
}
