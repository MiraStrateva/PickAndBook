using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Moq;
using NUnit.Framework;
using PickAndBook.Auth;
using PickAndBook.Controllers;
using PickAndBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.AccountControllerTests
{
    [TestFixture]
    public class Register_Should
    {
        [Test]
        public void RedirectToHomeIndex_WhenUserRegisteredAndGivenRoleClient()
        {
            // Arrange
            var viewModel = new RegisterViewModel();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStoreMock.Object);
            var authenticationManagerMock = new Mock<IAuthenticationManager>();
            var signInManagerMock = new Mock<ApplicationSignInManager>(userManagerMock.Object, authenticationManagerMock.Object);
            
            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Success));
            userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Success));
            var accountController = new AccountController(userManagerMock.Object, signInManagerMock.Object);

            // Act & Assert
            accountController.WithCallTo(c => c.Register(viewModel))
                .ShouldRedirectTo<HomeController>(c2 => c2.Index());
        }

        [Test]
        public void ReturnDefaultView_WhenUserRegisterIsNotSuccessful()
        {
            // Arrange
            var viewModel = new RegisterViewModel();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStoreMock.Object);
            var authenticationManagerMock = new Mock<IAuthenticationManager>();
            var signInManagerMock = new Mock<ApplicationSignInManager>(userManagerMock.Object, authenticationManagerMock.Object);

            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Success));
            userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Failed(new string[] { "Error 1", "Error 2" })));

            var accountController = new AccountController(userManagerMock.Object, signInManagerMock.Object);
            var expectedViewModel = new RegisterViewModel();

            // Act & Assert
            accountController.WithCallTo(c => c.Register(expectedViewModel))
                .ShouldRenderDefaultView()
                .WithModel(expectedViewModel);
        }

        [Test]
        public void ReturnDefaultView_WhenUserClientRoleAssignmentIsNotSuccessful()
        {
            // Arrange
            var viewModel = new RegisterViewModel();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStoreMock.Object);
            var authenticationManagerMock = new Mock<IAuthenticationManager>();
            var signInManagerMock = new Mock<ApplicationSignInManager>(userManagerMock.Object, authenticationManagerMock.Object);

            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Failed(new string[] { "AA", "BB" })));

            var accountController = new AccountController(userManagerMock.Object, signInManagerMock.Object);
            var expectedViewModel = new RegisterViewModel();

            // Act & Assert
            accountController.WithCallTo(c => c.Register(expectedViewModel))
                .ShouldRenderDefaultView()
                .WithModel(expectedViewModel);
        }

        [Test]
        public void ReturnDefaultView_WhenModelStateIsInvalid()
        {
            // Arrange
            var accountController = new AccountController();
            var expected = new RegisterViewModel();
            accountController.ModelState.AddModelError("Error", "Test Error");

            // Act & Assert
            accountController.WithCallTo(c => c.Register(expected))
                .ShouldRenderDefaultView()
                .WithModel(expected);
        }
    }
}
