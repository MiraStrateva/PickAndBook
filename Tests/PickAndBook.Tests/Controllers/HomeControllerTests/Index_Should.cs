using PickAndBook.Controllers;
using NUnit.Framework;
using PickAndBook.Data.Repositories.Contracts;
using Moq;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void ReturnDefaultView_WhenGetToIndex()
        {
            // Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            HomeController controller = new HomeController(categoryRepositoryMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.Index()).ShouldRenderDefaultView();
        }
    }
}
