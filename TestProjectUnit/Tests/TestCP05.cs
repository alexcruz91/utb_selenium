using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProjectUnit.Base;
using TestProjectUnit.Models;
using TestProjectUnit.Pages;

namespace TestProjectUnit.Tests
{
    [Category("Selenium")]
    public class TestCP05 : BaseTest
    {
        [Test]
        public void ValidacionBtnRegistrar()
        {
            var registroPage = new RegistroPage(driver);
            //CASO DE PRUEBA CP05: Validacion Boton Registrar
            UsuarioRegistro usuarioRegistro = new ();
            usuarioRegistro.Nombres = "";
            usuarioRegistro.Apellidos = "";
            usuarioRegistro.Correo = "";
            usuarioRegistro.Password = "";
            usuarioRegistro.ConfirmPassword = "";

            registroPage.Registrar(usuarioRegistro);

            WebDriverWait wait = new WebDriverWait(driver,TimeSpan.FromSeconds(10));

            wait.Until(d => d.FindElement(By.Id("nombres-error")).Displayed);

            wait.Until(d => d.FindElement(By.Id("apellidos-error")).Displayed);

            wait.Until(d => d.FindElement(By.Id("correo-error")).Displayed);

            wait.Until(d => d.FindElement(By.Id("password-error")).Displayed);


            Assert.Multiple(() =>
            {
                Assert.That(driver.FindElement(By.Id("nombres-error")).Text, Is.Not.Empty);

                Assert.That(driver.FindElement(By.Id("apellidos-error")).Text, Is.Not.Empty);

                Assert.That(driver.FindElement(By.Id("correo-error")).Text, Is.Not.Empty);

                Assert.That(driver.FindElement(By.Id("password-error")).Text, Is.Not.Empty);
            });
        }
    }
}

