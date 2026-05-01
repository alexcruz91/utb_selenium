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
using NUnit.Framework;

namespace TestProjectUnit.Tests
{
    public class TestCP01 : BaseTest
    {
        [Test]
        public void RegistroExitoso()
        {
            var registroPage = new RegistroPage(driver);
            var welcomePage = new WelcomePage(driver);
            //CASO DE PRUEBA CP01: Registro Exitoso
            UsuarioRegistro usuarioRegistro = new UsuarioRegistro();
            usuarioRegistro.Nombres = "Ana Milena";
            usuarioRegistro.Apellidos = "García Rovira";
            usuarioRegistro.Correo = "anamilegarcia@outlook.com";
            usuarioRegistro.Password = "3Sp4c14l2025++";
            usuarioRegistro.ConfirmPassword = "3Sp4c14l2025++";

            registroPage.Registrar(usuarioRegistro);

            //var welcome = new WelcomePage(driver);

            Assert.That(welcomePage.ObtenerTextoBienvenida(),Does.Contain("Registro Exitoso"));
        }
    }
}
