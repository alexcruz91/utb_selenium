using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestProjectUnit.Base
{
    public class BaseTest
    {
        protected IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            string url = Environment.GetEnvironmentVariable("TEST_URL")
             ?? "https://localhost:44387/Identity/Account/Register";
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(url);
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }
    }
}
