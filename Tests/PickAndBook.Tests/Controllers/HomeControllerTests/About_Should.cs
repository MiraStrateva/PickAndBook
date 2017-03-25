using Moq;
using NUnit.Framework;
using PickAndBook.Controllers;
using PickAndBook.Data;
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
            var pickAndBookDataMock = new Mock<IPickAndBookData>();
            HomeController controller = new HomeController(pickAndBookDataMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.About()).ShouldRenderDefaultView();
        }

        [Test]
        public void ReturnAboutView_WhenGetToAbout()
        {
            // Arrange
            var pickAndBookDataMock = new Mock<IPickAndBookData>();
            HomeController controller = new HomeController(pickAndBookDataMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.About()).ShouldRenderView("About") ;
        }
    }
}
