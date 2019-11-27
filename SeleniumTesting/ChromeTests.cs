using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;


namespace SeleniumTesting
{
    public class ChromeTests
    {
        [Fact]
        public void TestWithChromeDriver()
        {
            using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
            {
                driver.Manage().Window.Size = new Size(1280, 720);

                driver.Navigate().GoToUrl(@"https://automatetheplanet.com/multiple-files-page-objects-item-templates/");
                
                int topNavigationBarHeight = 100;

                var linkToClickSelector = By.PartialLinkText("WebDriver");
                var link = driver.FindElement(linkToClickSelector);

                var jsToBeExecuted = $"window.scroll(0, {link.Location.Y - topNavigationBarHeight});";
                driver.ExecuteScript(jsToBeExecuted);

                var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
                var clickableElement = wait.Until(ExpectedConditions.ElementIsVisible(linkToClickSelector));
                clickableElement.Click();
            }
        }

        [Fact]
        public void BasicAuthentication()
        {
            using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
            {
                driver.Navigate().GoToUrl(@"https://s2.demo.opensourcecms.com/orangehrm/symfony/web/index.php/auth/login");

                var userNameInput = driver.FindElement(By.Id("txtUsername"));
                var passwordInput = driver.FindElement(By.Id("txtPassword"));

                userNameInput.SendKeys("opensourcecms");
                passwordInput.SendKeys("opensourcecms");

                var loginButton = driver.FindElement(By.Id("btnLogin"));
                loginButton.Click();
            }
        }

        [Fact]
        public void AddElements()
        {
            using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
            {
                int elementsToAdd = 5;
                driver.Navigate().GoToUrl(@"http://the-internet.herokuapp.com/add_remove_elements/");

                var addButton = driver.FindElement(By.XPath("//button[text()='Add Element']"));

                for(var i = 0; i < elementsToAdd; i++)
                {
                    addButton.Click();
                }

                var addedElements = driver.FindElements(By.XPath("//div[@id='elements']/button"));

                Assert.True(elementsToAdd == addedElements.Count);
            }
        }

        [Fact]
        public void AddDressToCart()
        {
            using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
            {
                string expectedCartProductAmount = "1";

                driver.Navigate().GoToUrl(@"http://automationpractice.com/index.php");

                var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
                var linkSelector = By.XPath("//a[@title='Printed Dress']");
                wait.Until(ExpectedConditions.ElementToBeClickable(linkSelector));

                driver.FindElement(linkSelector).Click();

                var addToCartButtonSelector = By.XPath("//button[@type='submit' and span='Add to cart']");
                wait.Until(ExpectedConditions.ElementToBeClickable(addToCartButtonSelector));
                driver.FindElement(addToCartButtonSelector).Click();

                var closeButtonSelector = By.XPath("//span[@class='cross']");
                wait.Until(ExpectedConditions.ElementToBeClickable(closeButtonSelector));
                driver.FindElement(closeButtonSelector).Click();

                var cartProductsAmountSelector = By.XPath("//span[@class='ajax_cart_quantity unvisible']");
                wait.Until(ExpectedConditions.TextToBePresentInElement(driver.FindElement(cartProductsAmountSelector), expectedCartProductAmount));
                var productAmount = driver.FindElement(cartProductsAmountSelector).Text;

                Assert.True(expectedCartProductAmount == productAmount);
            }
        }


    }
}
