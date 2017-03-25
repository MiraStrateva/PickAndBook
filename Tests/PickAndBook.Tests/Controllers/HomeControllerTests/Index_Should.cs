using PickAndBook.Controllers;
using NUnit.Framework;
using Moq;
using TestStack.FluentMVCTesting;
using PickAndBook.Data;

namespace PickAndBook.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void ReturnDefaultView_WhenGetToIndex()
        {
            // Arrange
            var pickAndBookDataMock = new Mock<IPickAndBookData>();
            HomeController controller = new HomeController(pickAndBookDataMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.Index()).ShouldRenderDefaultView();
        }
    }
}
