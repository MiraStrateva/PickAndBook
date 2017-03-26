using Moq;
using NUnit.Framework;
using PickAndBook.Data.Common;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories;
using PickAndBook.Data.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PickAndBook.Data.Tests.CompanyRepositoryTests
{
    [TestFixture]
    public class GetLastAddedCompanies_Should
    {
        [TestCase(0)]
        [TestCase(DataConstants.LastRegisteredCompaniesCount)]
        [TestCase(DataConstants.LastRegisteredCompaniesCount - 1)]
        [TestCase(DataConstants.LastRegisteredCompaniesCount + 5)]
        public void ReturnExpectedResult_WhenCompaniesAreLessMoreEqualThan_LastRegisteredCompaniesCount(int count)
        {
            // Arrange
            var contextMock = new Mock<IPickAndBookDbContext>();
            var companySetMock = new Mock<IDbSet<Company>>();
            contextMock.Setup(c => c.Set<Company>()).Returns(companySetMock.Object);

            IEnumerable<Company> companies = GetCompanies(count);
            var companyDbSetMock = QueryableDbSetMock.GetQueryableMockDbSet(companies);
            contextMock.Setup(c => c.Set<Company>()).Returns(companyDbSetMock);

            CompanyRepository companyRepository = new CompanyRepository(contextMock.Object);

            var expectedCompanies = companyDbSetMock
                       .OrderByDescending(c => c.CompanyId)
                       .Take(DataConstants.LastRegisteredCompaniesCount);

            // Act 
            var resultCompanies = companyRepository.GetLastAddedCompanies();

            // Assert
            CollectionAssert.AreEqual(expectedCompanies, resultCompanies);
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
