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
            var options = new ChromeOptions();

            options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--remote-allow-origins=*");
            driver = new ChromeDriver(options);
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
            }
        }
    }
}
