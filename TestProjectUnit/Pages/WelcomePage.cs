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
    public class WelcomePage
    {
        private IWebDriver driver;

        public WelcomePage(IWebDriver driver)
        {
            this.driver = driver;
        }
        private IWebElement Mensaje => WaitHelper.WaitForElement(driver, By.Id("mensajerespuesta"));
        public string ObtenerTextoBienvenida()
        {
            return Mensaje.Text;
        }
    }
}
