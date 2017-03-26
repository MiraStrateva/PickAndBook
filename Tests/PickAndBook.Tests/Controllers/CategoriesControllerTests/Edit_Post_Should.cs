using Moq;
using NUnit.Framework;
using PickAndBook.Areas.Admin.Controllers;
using PickAndBook.Common;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using PickAndBook.Helpers;
using PickAndBook.Tests.Helpers;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.CategoriesControllerTests
{
    [TestFixture]
    public class Edit_Post_Should
    {

        [TestCase(typeof(HttpPostAttribute))]
        [TestCase(typeof(ValidateAntiForgeryTokenAttribute))]
        public void Have_RequiredAttributes(Type attrType)
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            CategoriesController controller = new CategoriesController(dataMock.Object);

            // Act
            var hasAttribute = AttributeTester.MethodHasAttribute(() => controller.Edit(It.IsAny<Category>(), null), attrType);

            // Assert
            Assert.IsTrue(hasAttribute);
        }

        [Test]
        public void ReturnDefaultViewWithModel_WhenStateIsNotValid()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            CategoriesController controller = new CategoriesController(dataMock.Object);
            controller.ModelState.AddModelError("test", "test");
            Category categoryNotValid = new Category()
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = "",
                CategoryDescription = "",
                CategoryImage = ""
            };

            // Act & Assert
            controller.WithCallTo(c => c.Edit(categoryNotValid, null))
                .ShouldRenderDefaultView()
                .WithModel(categoryNotValid);
        }

        [Test]
        public void RedirectToIndex_WhenStateIsValidWithoutUploadedImage()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            CategoriesController controller = new CategoriesController(dataMock.Object);
            Category category = new Category()
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = "Name 1",
                CategoryDescription = "Name 2",
                CategoryImage = "Image 1"
            };

            // Act & Assert
            controller.WithCallTo(c => c.Edit(category, null))
                .ShouldRedirectTo(c => c.Index(0, Constants.DefaultPageSize));
        }

        [Test]
        public void RedirectToIndex_WhenStateIsValidWithUploadedImage()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            var postedFileMock = new Mock<HttpPostedFileBase>();
            using (var stream = new MemoryStream())
            using (var bmp = new Bitmap(1, 1))
            {
                var graphics = Graphics.FromImage(bmp);
                graphics.FillRectangle(Brushes.Black, 0, 0, 1, 1);
                bmp.Save(stream, ImageFormat.Jpeg);

                postedFileMock.Setup(pf => pf.InputStream).Returns(stream);
                postedFileMock.Setup(pf => pf.ContentLength).Returns((int)stream.Length);
                postedFileMock.Setup(pf => pf.FileName).Returns("TestImage");
            }

            CategoriesController controller = new CategoriesController(dataMock.Object, new FileUploader(new TestPathProvider()));
            Category category = new Category()
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = "Name 1",
                CategoryDescription = "Name 2",
                CategoryImage = "Image 1"
            };

            // Act & Assert
            controller.WithCallTo(c => c.Edit(category, postedFileMock.Object))
                .ShouldRedirectTo(c => c.Index(0, Constants.DefaultPageSize));
        }

        [Test]
        public void ReturnDefaultViewWithModel_WhenImageUploadDoesNotSucceed()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            var postedFileMock = new Mock<HttpPostedFileBase>();
            using (var stream = new MemoryStream())
            using (var bmp = new Bitmap(1, 1))
            {
                var graphics = Graphics.FromImage(bmp);
                graphics.FillRectangle(Brushes.Black, 0, 0, 1, 1);
                bmp.Save(stream, ImageFormat.Jpeg);

                postedFileMock.Setup(pf => pf.InputStream).Returns(stream);
                postedFileMock.Setup(pf => pf.ContentLength).Returns((int)stream.Length);
                postedFileMock.Setup(pf => pf.FileName).Returns("TestImage");
            }
            postedFileMock.Setup(pf => pf.SaveAs(It.IsAny<String>()))
                        .Throws(new Exception());

            CategoriesController controller = new CategoriesController(dataMock.Object, new FileUploader(new TestPathProvider()));
            Category category = new Category()
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = "Name 1",
                CategoryDescription = "Name 2",
                CategoryImage = "Image 1"
            };

            // Act & Assert
            controller.WithCallTo(c => c.Edit(category, postedFileMock.Object))
                .ShouldRenderDefaultView()
                .WithModel(category);
        }
    }
}
