using Moq;
using NUnit.Framework;
using PickAndBook.Controllers;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.CompanyControllerTests
{
    [TestFixture]
    public class Index_Should
    {
        [Test]
        public void RedirectToHome_WhenCompanyForCurrentUserNotFound()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(c => c.GetCompanyByUserId(It.IsAny<String>())).Returns<Company>(null);
            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);
            
            string userId = Guid.NewGuid().ToString();
            var identity = new GenericIdentity(userId, "");
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, userId);
            identity.AddClaim(nameIdentifierClaim);
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.Identity).Returns(identity);
            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Setup(cc => cc.HttpContext.User).Returns(userMock.Object);

            CompanyController controller = new CompanyController(dataMock.Object)
            {
                ControllerContext = controllerContextMock.Object
            };

            // Act & Assert
            controller.WithCallTo(c => c.Index())
                .ShouldRedirectTo<HomeController>(c2 => c2.Index());
        }

        [Test]
        public void ReturnDefaultViewWithModel_WhenCompanyForCurrentUserFound()
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

            CompanyController controller = new CompanyController(dataMock.Object)
            {
                ControllerContext = controllerContextMock.Object
            };

            // Act & Assert
            controller.WithCallTo(c => c.Index())
                .ShouldRenderDefaultView()
                .WithModel<Company>(company);
        }
    }
}
