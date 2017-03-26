using Moq;
using NUnit.Framework;
using PickAndBook.Controllers;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using System;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.CompanyAnonymousControllerTests
{
    [TestFixture]
    public class Detail_Should
    {
        [Test]
        public void ReturnDefaultViewWithExpectedModel_WhenCompanyFound()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            Guid companyId = Guid.NewGuid();
            Company company = new Company()
            {
                CompanyId = companyId,
                Address = "Adress",
                City = "City",
                CategoryId = Guid.NewGuid(),
                CompanyDescription = "Description",
                CompanyName = "Name",
                Email = "Email",
                PhoneNumber = "889966558855",
                UserId = Guid.NewGuid().ToString()
            };
            companyRepositoryMock.Setup(c => c.GetById(companyId)).Returns(company);
            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);
           
            CompanyAnonymousController controller = new CompanyAnonymousController(dataMock.Object);

            // Act & Assert
            controller.WithCallTo(c => c.Detail(companyId))
                .ShouldRenderDefaultView()
                .WithModel<Company>(company);
        }

        [Test]
        public void RedirectToHomeIndex_WhenCompanyNotFound()
        {
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(c => c.GetById(It.IsAny<Guid>())).Returns<Company>(null);

            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);

            CompanyAnonymousController controller = new CompanyAnonymousController(dataMock.Object);

            // Act & Assert
            controller.WithCallTo(c => c.Detail(Guid.NewGuid()))
                .ShouldRedirectTo<HomeController>(c2 => c2.Index());
        }
    }
}
