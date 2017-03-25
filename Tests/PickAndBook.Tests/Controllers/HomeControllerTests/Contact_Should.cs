using NUnit.Framework;
using Moq;
using PickAndBook.Controllers;
using TestStack.FluentMVCTesting;
using PickAndBook.Data;

namespace PickAndBook.Tests.Controllers.HomeControllerTests
{
    [TestFixture]
    public class Contact_Should
    {
        [Test]
        public void ReturnDefaultView_WhenGetToContact()
        {
            // Arrange
            var pickAndBookDataMock = new Mock<IPickAndBookData>();
            HomeController controller = new HomeController(pickAndBookDataMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.Contact()).ShouldRenderDefaultView();
        }

        [Test]
        public void ReturnContactView_WhenGetToContact()
        {
            // Arrange
            var pickAndBookDataMock = new Mock<IPickAndBookData>();
            HomeController controller = new HomeController(pickAndBookDataMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.Contact()).ShouldRenderView("Contact");
        }
    }
}
