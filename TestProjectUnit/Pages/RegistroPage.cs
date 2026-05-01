using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            BtnRegistrar.Click();
        }
    }
}
