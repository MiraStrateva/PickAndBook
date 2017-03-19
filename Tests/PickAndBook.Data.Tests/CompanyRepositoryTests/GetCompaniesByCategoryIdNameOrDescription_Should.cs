﻿using Moq;
using NUnit.Framework;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories;
using PickAndBook.Data.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PickAndBook.Data.Tests.CompanyRepositoryTests
{
    [TestFixture]
    public class GetCompaniesByCategoryIdNameOrDescription_Should
    {
        [Test]
        public void RerurnCorrectResults_WhenTitleMatches()
        {
            // Arrange
            var contextMock = new Mock<IPickAndBookDbContext>();
            Guid[] categoryIds = new Guid[2];
            Guid testCategoryId = Guid.NewGuid();

            categoryIds[0] = testCategoryId;
            categoryIds[1] = Guid.NewGuid();

            IEnumerable<Company> companies = GetCompanies(categoryIds);
            string searchTerm = "Name";
            var expectedCompanyResultSet = companies.Where(c => c.CategoryId.Equals(testCategoryId) &&
                                c.CompanyName.ToLower().Contains(searchTerm)).AsQueryable();

            var companySetMock = QueryableDbSetMock.GetQueryableMockDbSet(companies);
            contextMock.Setup(c => c.Set<Company>()).Returns(companySetMock);

            CompanyRepository companyRepository = new CompanyRepository(contextMock.Object);

            // Act
            var companyResultSet = companyRepository.GetCompaniesByCategoryIdNameOrDescription(testCategoryId, searchTerm);

            // Assert
            CollectionAssert.AreEquivalent(expectedCompanyResultSet, companyResultSet);
        }

        [Test]
        public void RerurnCorrectResults_WhenDescriptionMatches()
        {
            // Arrange
            var contextMock = new Mock<IPickAndBookDbContext>();
            Guid[] categoryIds = new Guid[2];
            Guid testCategoryId = Guid.NewGuid();

            categoryIds[0] = testCategoryId;
            categoryIds[1] = Guid.NewGuid();

            IEnumerable<Company> companies = GetCompanies(categoryIds);
            string searchTerm = "desCription 1";
            var expectedCompanyResultSet = companies.Where(c => c.CategoryId.Equals(testCategoryId) &&
                                c.CompanyDescription.ToLower().Contains(searchTerm)).AsQueryable();

            var companySetMock = QueryableDbSetMock.GetQueryableMockDbSet(companies);
            contextMock.Setup(c => c.Set<Company>()).Returns(companySetMock);

            CompanyRepository companyRepository = new CompanyRepository(contextMock.Object);

            // Act
            var companyResultSet = companyRepository.GetCompaniesByCategoryIdNameOrDescription(testCategoryId, searchTerm);

            // Assert
            CollectionAssert.AreEquivalent(expectedCompanyResultSet, companyResultSet);
        }


        private IEnumerable<Company> GetCompanies(Guid[] categoryIds)
        {
            List<Company> companies = new List<Company>();
            int index = 1;

            foreach (Guid categoryId in categoryIds)
            {
                for (int i = index; i < index + 3; i++)
                {
                    companies.Add(new Company()
                    {
                        CompanyId = Guid.NewGuid(),
                        CompanyName = "Name " + i,
                        CompanyDescription = "Description " + i,
                        CategoryId = categoryId
                    });
                }
                index += 3;
            }

            return companies;
        }
    }
}
