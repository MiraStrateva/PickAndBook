using Moq;
using NUnit.Framework;
using PickAndBook.Controllers;
using PickAndBook.Data.Repositories.Contracts;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.HomeControllerTests
{
    [TestFixture]
    public class About_Should
    {
        [Test]
        public void ReturnDefaultView_WhenGetToAbout()
        {
            // Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            HomeController controller = new HomeController(categoryRepositoryMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.About()).ShouldRenderDefaultView();
        }

        [Test]
        public void ReturnAboutView_WhenGetToAbout()
        {
            // Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            HomeController controller = new HomeController(categoryRepositoryMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.About()).ShouldRenderView("About") ;
        }
    }
}
