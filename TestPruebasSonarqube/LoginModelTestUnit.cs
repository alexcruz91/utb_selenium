
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using PruebasMetricasProject.Areas.Identity.Pages.Account;
using PruebasMetricasProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPruebasSonarqube
{
    [TestFixture]
    public class LoginModelTestUnit
    {
        private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private Mock<ILogger<LoginModel>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _signInManagerMock = MockSignInManager();
            _loggerMock = new Mock<ILogger<LoginModel>>();
        }

        [Test]
        public async Task OnPostAsync_LoginCorrecto_Redirecciona()
        {
            // Arrange

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var model = new LoginModel(
                _signInManagerMock.Object,
                _loggerMock.Object);

            ConfigurarContexto(model);

            model.Input = new LoginModel.InputModel
            {
                Email = "test@test.com",
                Password = "Password123!",
                RememberMe = false
            };

            // Act

            var result = await model.OnPostAsync();

            // Assert

            Assert.That(result, Is.InstanceOf<LocalRedirectResult>());

            _signInManagerMock.Verify(x =>
                x.PasswordSignInAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    false),
                Times.Once);
        }

        [Test]
        public async Task OnPostAsync_LoginInvalido_RetornaPage()
        {
            // Arrange

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var model = new LoginModel(
                _signInManagerMock.Object,
                _loggerMock.Object);

            ConfigurarContexto(model);

            model.Input = new LoginModel.InputModel
            {
                Email = "test@test.com",
                Password = "IncorrectPassword",
                RememberMe = false
            };

            // Act

            var result = await model.OnPostAsync();

            // Assert

            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        [Test]
        public async Task OnPostAsync_UsuarioBloqueado_RedireccionaLockout()
        {
            // Arrange

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

            var model = new LoginModel(
                _signInManagerMock.Object,
                _loggerMock.Object);

            ConfigurarContexto(model);

            model.Input = new LoginModel.InputModel
            {
                Email = "test@test.com",
                Password = "Password123!",
                RememberMe = false
            };

            // Act

            var result = await model.OnPostAsync();

            // Assert

            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        [Test]
        public async Task OnPostAsync_RequiresTwoFactor_Redirecciona2FA()
        {
            // Arrange

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.TwoFactorRequired);

            var model = new LoginModel(
                _signInManagerMock.Object,
                _loggerMock.Object);

            ConfigurarContexto(model);

            model.Input = new LoginModel.InputModel
            {
                Email = "test@test.com",
                Password = "Password123!",
                RememberMe = true
            };

            // Act

            var result = await model.OnPostAsync();

            // Assert

            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        [Test]
        public async Task OnPostAsync_ModeloInvalido_RetornaPage()
        {
            // Arrange

            var model = new LoginModel(
                _signInManagerMock.Object,
                _loggerMock.Object);

            ConfigurarContexto(model);

            model.ModelState.AddModelError("Email", "Campo requerido");

            // Act

            var result = await model.OnPostAsync();

            // Assert

            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        private Mock<SignInManager<ApplicationUser>> MockSignInManager()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                userStore.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

            return new Mock<SignInManager<ApplicationUser>>(
                userManager.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null,
                null,
                null,
                null);
        }

        private void ConfigurarContexto(LoginModel model)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Scheme = "https";

            var actionContext = new ActionContext(
                httpContext,
                new RouteData(),
                new ActionDescriptor());

            model.PageContext = new PageContext(actionContext);

            var tempData = new TempDataDictionary(
                httpContext,
                Mock.Of<ITempDataProvider>());

            model.TempData = tempData;

            var urlHelperMock = new Mock<IUrlHelper>();

            urlHelperMock.Setup(x => x.Content(It.IsAny<string>()))
                         .Returns("/");

            model.Url = urlHelperMock.Object;
        }
    }
}
