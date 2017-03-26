using Moq;
using NUnit.Framework;
using PickAndBook.Controllers;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.CompanyAnonymousControllerTests
{
    [TestFixture]
    public class ByCategory_Should
    {
        [Test]
        public void ShouldRedirectToIndexPage_WhenNoCompaniesFound()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();

            companyRepositoryMock.Setup(m => m.GetCompaniesByCategoryId(It.IsAny<Guid>())).Returns<IQueryable<Company>>(null);
            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);

            CompanyAnonymousController controller = new CompanyAnonymousController(dataMock.Object);

            // Act & Assert
            controller.WithCallTo(c => c.ByCategory(Guid.NewGuid()))
                .ShouldRedirectTo<HomeController>(c2 => c2.Index());
        }

        [Test]
        public void ShouldReturnIndexViewWithExpectedModel_WhenCompaniesFound()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var companies = GetCompanies(2);
            companyRepositoryMock.Setup(m => m.GetCompaniesByCategoryId(It.IsAny<Guid>())).Returns(companies.AsQueryable());

            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);

            CompanyAnonymousController controller = new CompanyAnonymousController(dataMock.Object);

            // Act & Assert
            controller.WithCallTo(c => c.ByCategory(Guid.NewGuid()))
                .ShouldRenderView("Index")
                .WithModel<List<Company>>();

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
