using Moq;
using NUnit.Framework;
using PickAndBook.Controllers;
using PickAndBook.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PickAndBook.Tests.Helpers;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using TestStack.FluentMVCTesting;
using PickAndBook.Helpers;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Principal;
using System.Security.Claims;

namespace PickAndBook.Tests.Controllers.RegisterCompanyControllerTests
{
    [TestFixture]
    public class Index_Post_Should
    {
        [TestCase(typeof(HttpPostAttribute))]
        [TestCase(typeof(ValidateAntiForgeryTokenAttribute))]
        public void Have_RequiredAttributes(Type attrType)
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            RegisterCompanyController controller = new RegisterCompanyController(dataMock.Object);

            // Act
            var hasAttribute = AttributeTester.MethodHasAttribute(() => controller.Index(It.IsAny<Company>(), null), attrType);

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

            RegisterCompanyController controller = new RegisterCompanyController(dataMock.Object);
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
            controller.WithCallTo(c => c.Index(companyNotValid, null))
                .ShouldRenderDefaultView()
                .WithModel(companyNotValid);
        }

        [Test]
        public void ReturnCompanyDetailsWithCompanyModel_WhenStateIsValidWithoutUploadedImageAndRoleChanged()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);

            string userId = Guid.NewGuid().ToString();
            var identity = new GenericIdentity(userId, "");
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, userId);
            identity.AddClaim(nameIdentifierClaim);
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.Identity).Returns(identity);
            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Setup(cc => cc.HttpContext.User).Returns(userMock.Object);

            RegisterCompanyController controller = new RegisterCompanyController(dataMock.Object, new FileUploader(new TestPathProvider()),
                new TestUserRoleManagerTrue())
            {
                ControllerContext = controllerContextMock.Object
            };

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
            controller.WithCallTo(c => c.Index(company, null))
                .ShouldRenderView("CompanyDetails")
                .WithModel<Company>(company);
        }

        [Test]
        public void ReturnDefaultViewWithModel_WhenStateIsValidWithoutUploadedImageAndRoleNotChanged()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var categoriesAll = GetCategories(5);
            categoryRepositoryMock.Setup(c => c.All()).Returns(categoriesAll.AsQueryable());
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            string userId = Guid.NewGuid().ToString();
            var identity = new GenericIdentity(userId, "");
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, userId);
            identity.AddClaim(nameIdentifierClaim);
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.Identity).Returns(identity);
            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Setup(cc => cc.HttpContext.User).Returns(userMock.Object);

            RegisterCompanyController controller = new RegisterCompanyController(dataMock.Object, new FileUploader(new TestPathProvider()),
                new TestUserRoleManagerFalse())
            {
                ControllerContext = controllerContextMock.Object
            };

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
            controller.WithCallTo(c => c.Index(company, null))
                .ShouldRenderDefaultView()
                .WithModel<Company>(company);
        }

        [Test]
        public void ReturnCompanyDetailsWithCompanyModel_WhenStateIsValidWithUploadedImageAndRoleChanged()
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

            string userId = Guid.NewGuid().ToString();
            var identity = new GenericIdentity(userId, "");
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, userId);
            identity.AddClaim(nameIdentifierClaim);
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.Identity).Returns(identity);
            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Setup(cc => cc.HttpContext.User).Returns(userMock.Object);

            RegisterCompanyController controller = new RegisterCompanyController(dataMock.Object, new FileUploader(new TestPathProvider()),
                new TestUserRoleManagerTrue())
            {
                ControllerContext = controllerContextMock.Object
            };
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
            controller.WithCallTo(c => c.Index(company, postedFileMock.Object))
                .ShouldRenderView("CompanyDetails")
                .WithModel<Company>(company);
        }

        [Test]
        public void ReturnDefaultViewWithModel_WhenStateIsValidWithUploadedImageAndRoleNotChanged()
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

            string userId = Guid.NewGuid().ToString();
            var identity = new GenericIdentity(userId, "");
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, userId);
            identity.AddClaim(nameIdentifierClaim);
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.Identity).Returns(identity);
            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Setup(cc => cc.HttpContext.User).Returns(userMock.Object);

            RegisterCompanyController controller = new RegisterCompanyController(dataMock.Object, new FileUploader(new TestPathProvider()),
                new TestUserRoleManagerFalse())
            {
                ControllerContext = controllerContextMock.Object
            };

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
            controller.WithCallTo(c => c.Index(company, postedFileMock.Object))
                .ShouldRenderDefaultView()
                .WithModel<Company>(company);
        }

        [Test]
        public void ReturnDefaultViewWithModel_WhenImageUploadDoesNotSucceed()
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
            postedFileMock.Setup(pf => pf.SaveAs(It.IsAny<String>()))
                        .Throws(new Exception());

            RegisterCompanyController controller = new RegisterCompanyController(dataMock.Object, new FileUploader(new TestPathProvider()),
                new TestUserRoleManagerTrue());

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
            controller.WithCallTo(c => c.Index(company, postedFileMock.Object))
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
