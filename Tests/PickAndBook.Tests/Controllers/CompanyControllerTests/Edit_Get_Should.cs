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

namespace PickAndBook.Tests.Controllers.CompanyControllerTests
{
    [TestFixture]
    public class Edit_Get_Should
    {
        [Test]
        public void RedirectToHome_WhenCompanyNotFound()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(c => c.GetById(It.IsAny<Guid>())).Returns<Company>(null);
            dataMock.Setup(c => c.Companies).Returns(companyRepositoryMock.Object);

            CompanyController controller = new CompanyController(dataMock.Object);

            // Act & Assert
            controller.WithCallTo(c => c.Edit(Guid.NewGuid()))
                .ShouldRedirectTo<HomeController>(c2 => c2.Index());
        }

        [Test]
        public void ReturnDefaultViewWithModel_WhenCompanyFound()
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
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var categoriesAll = GetCategories(5);
            categoryRepositoryMock.Setup(c => c.All()).Returns(categoriesAll.AsQueryable());
            dataMock.Setup(c => c.Categories).Returns(categoryRepositoryMock.Object);

            CompanyController controller = new CompanyController(dataMock.Object);

            // Act & Assert
            controller.WithCallTo(c => c.Edit(companyId))
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
