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
    public class TestCP02 : BaseTest
    {
        [Test]
        public void ValidacionCorreo()
        {
            var registroPage = new RegistroPage(driver);
            var welcomePage = new WelcomePage(driver);
            //CASO DE PRUEBA CP02: Validacion de correo electronico
            UsuarioRegistro usuarioRegistro = new UsuarioRegistro();
            usuarioRegistro.Nombres = "Juan Manuel";
            usuarioRegistro.Apellidos = "Pérez Correa";
            usuarioRegistro.Correo = "juanperezoutlook.com";
            usuarioRegistro.Password = "C0l0mb142021*";
            usuarioRegistro.ConfirmPassword = "C0l0mb142021*";

            registroPage.Registrar(usuarioRegistro);

            //var mensaje = driver.FindElement(By.Id("correo-error")).Text;
            var mensaje = WaitHelper.WaitForElement(driver, By.Id("correo-error")).Text;
            //Assert.IsTrue(mensaje.Contains("The Email field is not a valid e-mail address."));
            Assert.That(mensaje, Does.Contain("The Email field is not a valid e-mail address."));
            


        }
    }
}
