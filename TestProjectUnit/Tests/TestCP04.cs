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
    [Category("Selenium")]
    public class TestCP04 : BaseTest
    {
        [Test]
        public void CorreoRegistrado()
        {
            var registroPage = new RegistroPage(driver);
            //CASO DE PRUEBA CP04: Validación de correo electrónico ya registrado
            UsuarioRegistro usuarioRegistro = new ();
            usuarioRegistro.Nombres = "Carla Tatiana";
            usuarioRegistro.Apellidos = "Ríos";
            usuarioRegistro.Correo = "carlarios@outlook.com";
            usuarioRegistro.Password = "H2)ehqlVlr$p_!+jx";
            usuarioRegistro.ConfirmPassword = "H2)ehqlVlr$p_!+jx";

            registroPage.Registrar(usuarioRegistro);

            var mensaje = WaitHelper.WaitForElement(driver, By.ClassName("validation-summary-errors")).Text;

            Assert.That(mensaje, Does.Contain("is already taken."));
        }
    }
}
