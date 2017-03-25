using NUnit.Framework;
using Moq;
using PickAndBook.Controllers;
using TestStack.FluentMVCTesting;
using PickAndBook.Data.Repositories.Contracts;

namespace PickAndBook.Tests.Controllers.HomeControllerTests
{
    [TestFixture]
    public class Contact_Should
    {
        [Test]
        public void ReturnDefaultView_WhenGetToContact()
        {
            // Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            HomeController controller = new HomeController(categoryRepositoryMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.Contact()).ShouldRenderDefaultView();
        }

        [Test]
        public void ReturnContactView_WhenGetToContact()
        {
            // Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            HomeController controller = new HomeController(categoryRepositoryMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.Contact()).ShouldRenderView("Contact");
        }
    }
}
