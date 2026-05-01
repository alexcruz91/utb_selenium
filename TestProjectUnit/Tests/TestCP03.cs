using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProjectUnit.Base;
using TestProjectUnit.Models;
using TestProjectUnit.Pages;
using TestProjectUnit.Utilities;

namespace TestProjectUnit.Tests
{
    public class TestCP03 : BaseTest
    {
        [Test]
        public void ValidacionPassword()
        {
            var registroPage = new RegistroPage(driver);
            var welcomePage = new WelcomePage(driver);
            //CASO DE PRUEBA CP03: Validacion de contraseña
            UsuarioRegistro usuarioRegistro = new UsuarioRegistro();
            usuarioRegistro.Nombres = "Luis Alfonso";
            usuarioRegistro.Apellidos = "Mora";
            usuarioRegistro.Correo = "luisalfomora@outlook.com";
            usuarioRegistro.Password = "12345678";
            usuarioRegistro.ConfirmPassword = "12345678";

            registroPage.Registrar(usuarioRegistro);

            //var mensaje = driver.FindElement(By.Id("correo-error")).Text;
            var mensaje = WaitHelper.WaitForElement(driver, By.ClassName("validation-summary-errors")).Text;

            Assert.That(mensaje, Does.Contain("Passwords must have"));
        }
    }
}
