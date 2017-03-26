using Moq;
using NUnit.Framework;
using PickAndBook.Controllers;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.RegisterCompanyControllerTests
{
    [TestFixture]
    public class Index_Get_Should
    {
        [Test]
        public void ReturnDefaultPageWhitCompanyModel_WhenCompanyForCurrentUserNotFound()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var categoriesAll = GetCategories(5);
            categoryRepositoryMock.Setup(c => c.All()).Returns(categoriesAll.AsQueryable());
            companyRepositoryMock.Setup(c => c.GetCompanyByUserId(It.IsAny<String>())).Returns<Company>(null);
            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            string userId = Guid.NewGuid().ToString();
            var identity = new GenericIdentity(userId, "");
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, userId);
            identity.AddClaim(nameIdentifierClaim);
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.Identity).Returns(identity);
            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Setup(cc => cc.HttpContext.User).Returns(userMock.Object);

            RegisterCompanyController controller = new RegisterCompanyController(dataMock.Object)
            {
                ControllerContext = controllerContextMock.Object
            };

            // Act & Assert
            controller.WithCallTo(c => c.Index())
                .ShouldRenderDefaultView()
                .WithModel<Company>();
        }

        [Test]
        public void ReturnCompanyDetailsPageWhitCompanyModel_WhenCompanyForCurrentUserFound()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            string userId = Guid.NewGuid().ToString();
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
                                    UserId = userId
                                };
            companyRepositoryMock.Setup(c => c.GetCompanyByUserId(userId)).Returns(company);
            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);

            var identity = new GenericIdentity(userId, "");
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, userId);
            identity.AddClaim(nameIdentifierClaim);
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.Identity).Returns(identity);
            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Setup(cc => cc.HttpContext.User).Returns(userMock.Object);

            RegisterCompanyController controller = new RegisterCompanyController(dataMock.Object)
            {
                ControllerContext = controllerContextMock.Object
            };

            // Act & Assert
            controller.WithCallTo(c => c.Index())
                .ShouldRenderView("CompanyDetails")
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
