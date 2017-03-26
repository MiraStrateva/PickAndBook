using Moq;
using NUnit.Framework;
using PickAndBook.Controllers;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using PickAndBook.Helpers;
using PickAndBook.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.CompanyControllerTests
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
            CompanyController controller = new CompanyController(dataMock.Object);

            // Act
            var hasAttribute = AttributeTester.MethodHasAttribute(() => controller.Edit(It.IsAny<Company>(), null), attrType);

            // Assert
            Assert.IsTrue(hasAttribute);
        }
        
        [Test]
        public void ReturnDefaultViewWithModel_WhenStateIsNotValid()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var categoriesAll = GetCategories(5);
            categoryRepositoryMock.Setup(c => c.All()).Returns(categoriesAll.AsQueryable());
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            CompanyController controller = new CompanyController(dataMock.Object);
            controller.ModelState.AddModelError("test", "test");
            Company companyNotValid = new Company()
            {
                CompanyId = Guid.NewGuid(),
                Address = "",
                City = "",
                CategoryId = Guid.NewGuid(),
                CompanyDescription = "",
                CompanyName = "",
                Email = "",
                PhoneNumber = "",
                UserId = ""
            };

            // Act & Assert
            controller.WithCallTo(c => c.Edit(companyNotValid, null))
                .ShouldRenderDefaultView()
                .WithModel(companyNotValid);
        }

        [Test]
        public void RedirectToCompanyIndexWithCompanyModel_WhenStateIsValidWithoutUploadedImage()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);

            CompanyController controller = new CompanyController(dataMock.Object);

            Company company = new Company()
            {
                CompanyId = Guid.NewGuid(),
                Address = "Adress",
                City = "City",
                CategoryId = Guid.NewGuid(),
                CompanyDescription = "Description",
                CompanyName = "Name",
                Email = "Email",
                PhoneNumber = "889966558855",
                UserId = Guid.NewGuid().ToString()
            };

            // Act & Assert
            controller.WithCallTo(c => c.Edit(company, null))
                .ShouldRedirectTo(a => a.Index);
        }

        [Test]
        public void RedirectToCompanyIndexWithCompanyModel_WhenStateIsValidWithtUploadedImage()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);

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

            CompanyController controller = new CompanyController(dataMock.Object, new FileUploader(new TestPathProvider()));
            Company company = new Company()
            {
                CompanyId = Guid.NewGuid(),
                Address = "Adress",
                City = "City",
                CategoryId = Guid.NewGuid(),
                CompanyDescription = "Description",
                CompanyName = "Name",
                Email = "Email",
                PhoneNumber = "889966558855",
                UserId = Guid.NewGuid().ToString()
            };

            // Act & Assert
            controller.WithCallTo(c => c.Edit(company, postedFileMock.Object))
                .ShouldRedirectTo(a => a.Index);
        }

        [Test]
        public void ReturnDefaultViewWithModel_WhenImageUploadDoesNotSucceed()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var categoriesAll = GetCategories(5);
            categoryRepositoryMock.Setup(c => c.All()).Returns(categoriesAll.AsQueryable());
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

            CompanyController controller = new CompanyController(dataMock.Object, new FileUploader(new TestPathProvider()));

            Company company = new Company()
            {
                CompanyId = Guid.NewGuid(),
                Address = "Adress",
                City = "City",
                CategoryId = Guid.NewGuid(),
                CompanyDescription = "Description",
                CompanyName = "Name",
                Email = "Email",
                PhoneNumber = "889966558855",
                UserId = Guid.NewGuid().ToString()
            };

            // Act & Assert
            controller.WithCallTo(c => c.Edit(company, postedFileMock.Object))
                .ShouldRenderDefaultView()
                .WithModel<Company>(company);
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
