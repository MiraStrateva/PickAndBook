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
    public class Index_Should
    {
        [Test]
        public void ReturnDefaultViewWithExpectedViewModelType_WhenGetToIndex()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            IEnumerable<Company> companies = GetCompanies(5);
            var expectedCompanyResultSet = companies.AsQueryable();

            companyRepositoryMock.Setup(m => m.All()).Returns(expectedCompanyResultSet);
            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);

            CompanyAnonymousController controller = new CompanyAnonymousController(dataMock.Object);

            // Act & Assert
            controller.WithCallTo(c => c.Index())
                .ShouldRenderDefaultView()
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
