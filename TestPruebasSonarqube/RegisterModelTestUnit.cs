using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using PruebasMetricasProject.Areas.Identity.Pages.Account;
using PruebasMetricasProject.Models;

namespace TestPruebasSonarqube
{
    [TestFixture]
    public class RegisterModelTestUnit
    {
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<IUserEmailStore<ApplicationUser>> _userStoreMock;
        private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private Mock<ILogger<RegisterModel>> _loggerMock;
        private Mock<IEmailSender> _emailSenderMock;

        [SetUp]
        public void Setup()
        {
            _userStoreMock = new Mock<IUserEmailStore<ApplicationUser>>();

            _userManagerMock = MockUserManager();

            _userManagerMock.Setup(x => x.SupportsUserEmail)
                    .Returns(true);

            _signInManagerMock = MockSignInManager();

            _signInManagerMock
                    .Setup(x => x.GetExternalAuthenticationSchemesAsync())
                    .ReturnsAsync(new List<AuthenticationScheme>());

            _loggerMock = new Mock<ILogger<RegisterModel>>();

            _emailSenderMock = new Mock<IEmailSender>();
        }
        [Test]
        public async Task OnPostAsync_ModeloValido_CreaUsuario()
        {
            // Arrange

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.GenerateEmailConfirmationTokenAsync(
                    It.IsAny<ApplicationUser>()))
                .ReturnsAsync("token-prueba");

            _userManagerMock
                .Setup(x => x.GetUserIdAsync(
                    It.IsAny<ApplicationUser>()))
                .ReturnsAsync("123");

            _signInManagerMock
                .Setup(x => x.SignInAsync(
                    It.IsAny<ApplicationUser>(),
                    false,
                    null))
                .Returns(Task.CompletedTask);

            var model = new RegisterModel(
                _userManagerMock.Object,
                _userStoreMock.Object,
                _signInManagerMock.Object,
                _loggerMock.Object,
                _emailSenderMock.Object);

            ConfigurarContexto(model);

            model.Input = new RegisterModel.InputModel
            {
                Nombre = "Andres",
                Apellido = "Lopez",
                Email = "test@test.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            // Act

            var result = await model.OnPostAsync();

            // Assert

            Assert.That(result, Is.Not.Null);

            _userManagerMock.Verify(x =>
                x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()),
                Times.Once);
        }

        private Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();

            return new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);
        }

        private Mock<SignInManager<ApplicationUser>> MockSignInManager()
        {
            var userManager = MockUserManager();

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
        [Test]
        public async Task OnPostAsync_ModeloInvalido_RetornaPage()
        {
            var model = new RegisterModel(
                _userManagerMock.Object,
                _userStoreMock.Object,
                _signInManagerMock.Object,
                _loggerMock.Object,
                _emailSenderMock.Object);

            ConfigurarContexto(model);

            model.ModelState.AddModelError("Email", "Requerido");

            var result = await model.OnPostAsync();

            Assert.That(result, Is.InstanceOf<PageResult>());
        }
        [Test]
        public async Task OnPostAsync_ErrorCreacion_RetornaPage()
        {
            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Error"
                    }));

            var model = new RegisterModel(
                _userManagerMock.Object,
                _userStoreMock.Object,
                _signInManagerMock.Object,
                _loggerMock.Object,
                _emailSenderMock.Object);

            ConfigurarContexto(model);

            model.Input = new RegisterModel.InputModel
            {
                Nombre = "Andres",
                Apellido = "Lopez",
                Email = "test@test.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            var result = await model.OnPostAsync();

            Assert.That(result, Is.InstanceOf<PageResult>());
        }
        private void ConfigurarContexto(RegisterModel model)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost");

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

            urlHelperMock.SetupGet(x => x.ActionContext)
                         .Returns(actionContext);

            urlHelperMock
                .Setup(x => x.Content(It.IsAny<string>()))
                .Returns("/");

            urlHelperMock
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("https://localhost/Account/ConfirmEmail");

            model.Url = urlHelperMock.Object;
        }
    }
}
