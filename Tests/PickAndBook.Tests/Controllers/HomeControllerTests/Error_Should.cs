using Moq;
using NUnit.Framework;
using PickAndBook.Controllers;
using PickAndBook.Data.Repositories.Contracts;
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
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            HomeController controller = new HomeController(categoryRepositoryMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.Error()).ShouldRenderView("Error");
        }
    }
}
