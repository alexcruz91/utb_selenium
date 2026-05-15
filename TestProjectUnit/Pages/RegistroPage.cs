using OpenQA.Selenium;
using TestProjectUnit.Models;
using TestProjectUnit.Utilities;

namespace TestProjectUnit.Pages
{
    public class RegistroPage
    {
        private IWebDriver driver;

        public RegistroPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        private IWebElement Nombres => WaitHelper.WaitForElement(driver, By.Id("nombres"));
        private IWebElement Apellidos => WaitHelper.WaitForElement(driver, By.Id("apellidos"));
        private IWebElement Correo => WaitHelper.WaitForElement(driver, By.Id("correo"));
        private IWebElement Password => WaitHelper.WaitForElement(driver, By.Id("password"));
        private IWebElement ConfirmPassword => WaitHelper.WaitForElement(driver, By.Id("confirmpassword"));
        private IWebElement BtnRegistrar => WaitHelper.WaitForElement(driver, By.Id("registerSubmit"));

        public void Registrar(UsuarioRegistro usuarioRegistro)
        {
            Nombres.SendKeys(usuarioRegistro.Nombres);
            Apellidos.SendKeys(usuarioRegistro.Apellidos);
            Correo.SendKeys(usuarioRegistro.Correo);
            Password.SendKeys(usuarioRegistro.Password);
            ConfirmPassword.SendKeys(usuarioRegistro.ConfirmPassword);
            ClickConFallback(BtnRegistrar);
        }

        // Click seguro: intenta normal, si falla usa JavaScript
        private void ClickConFallback(IWebElement elemento)
        {
            try
            {
                // Primero hace scroll al elemento para asegurar que esté visible
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].scrollIntoView({block: 'center'});", elemento);
                Thread.Sleep(300); // pequeña pausa tras el scroll
                elemento.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Si sigue bloqueado, click directo via JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].click();", elemento);
            }
        }
    }
}
