using Moq;
using NUnit.Framework;
using PickAndBook.Controllers;
using PickAndBook.Data;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.HomeControllerTests
{
    [TestFixture]
    public class Error_Should
    {
        [Test]
        public void ReturnErrorView_WhenGetToError()
        {
            // Arrange
            var pickAndBookDataMock = new Mock<IPickAndBookData>();
            HomeController controller = new HomeController(pickAndBookDataMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.Error()).ShouldRenderView("Error");
        }
    }
}
