using Moq;
using NUnit.Framework;
using PickAndBook.Controllers;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using PickAndBook.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.HomeControllerTests
{
    [TestFixture]
    public class LastAddedCompanies_Should
    {
        [Test]
        public void ReturnLastAddedCompaniesPartialView_WhenGetToLastAddedCompanies()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyServiceMock = new Mock<ICompanyService>();
            IEnumerable<Company> companies = GetCompanies(5);
            var expectedCompanyResultSet = companies.AsQueryable();

            companyRepositoryMock.Setup(m => m.All()).Returns(expectedCompanyResultSet);
            companyServiceMock.Setup(s => s.GetLastAddedCompanies()).Returns(expectedCompanyResultSet);
            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);

            HomeController controller = new HomeController(dataMock.Object, companyServiceMock.Object);

            // Act & Assert
            controller.WithCallTo(c => c.LastAddedCompanies())
                .ShouldRenderPartialView("_LastAddedCompanies");
        }

        [Test]
        public void ReturnLastAddedCompaniesPartialViewWithExpectedModelType_WhenGetToLastAddedCompanies()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyServiceMock = new Mock<ICompanyService>();
            IEnumerable<Company> companies = GetCompanies(5);
            var expectedCompanyResultSet = companies.AsQueryable();

            companyRepositoryMock.Setup(m => m.All()).Returns(expectedCompanyResultSet);
            companyServiceMock.Setup(s => s.GetLastAddedCompanies()).Returns(expectedCompanyResultSet);
            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);

            HomeController controller = new HomeController(dataMock.Object, companyServiceMock.Object);

            // Act & Assert
            controller.WithCallTo(c => c.LastAddedCompanies())
                .ShouldRenderPartialView("_LastAddedCompanies")
                .WithModel<IList<Company>>();
        }

        private IEnumerable<Company> GetCompanies(int count)
        {
            List<Company> companies = new List<Company>();

            for (int i = 0; i < count; i++)
            {
                companies.Add(new Company()
                {
                    CompanyId = Guid.NewGuid(),
                    CompanyName = "Name " + i,
                    CompanyDescription = "Description " + i,
                    CategoryId = Guid.NewGuid()
                });
            }

            return companies;
        }
    }
}
