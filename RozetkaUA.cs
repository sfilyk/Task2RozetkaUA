using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace Task2RozetkaUA
{
    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }
        public static bool elexists(By by, IWebDriver driver)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public static void waitforelement(IWebDriver driver, By by)
        {
            for (int i = 0; i < 30; i++)
            {
                Thread.Sleep(1000);
                if (elexists(by, driver))
                {
                    break;
                }
            }
        }

        //public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
        //{
        //    if (timeoutInSeconds > 0)
        //    {
        //        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        //        return wait.Until(drv => (drv.FindElements(by).Count > 0) ? drv.FindElements(by) : null);
        //    }
        //    return driver.FindElements(by);
        //}
    }
    public static class MyAssert
    {
        public static bool MyIsTrue(int first, string second)
        {            
            string numericString = "";
            foreach (char c in second)
            {
                // Check for numeric characters (0-9), a negative sign, or leading or trailing spaces.
                if ((c >= '0' && c <= '9') || c == ' ' || c == '-')
                {
                    numericString = String.Concat(numericString, c);
                }
                else  
                    break;                
            }
            if (first < Convert.ToInt32(numericString)) 
            {
                return true;
            }
            else 
                return false;
        }
    }
    
    public class Tests
    {
        private IWebDriver driver;
                
        private readonly By _buttonLinkMenuCategoriesNetboock = By.CssSelector("sidebar-fat-menu li:nth-child(1)");
        private readonly By _buttonIconFirstListProduct = By.XPath("//li[contains(@class,'portal')][1]");
        private readonly By _buttonIconSecondListProduct = By.CssSelector("section > ul > li:nth-child(2)");
        private readonly By _buttonBrendProduct = By.CssSelector("rz-filter-section-autocomplete  ul:nth-child(1) > li:nth-child(5)");
        private readonly By _buttonIconAddProductBay = By.XPath("//*[@class ='toOrder ng-star-inserted']");
        private readonly By _buttonChoiceByPrice = By.CssSelector("select[_ngcontent-rz-client-c41='']");
        private readonly By _buttonChoiceByPriceExpensive = By.CssSelector("select[_ngcontent-rz-client-c41=''] option[value='2: expensive']");
        private readonly By _buttonProductsInCart = By.XPath("//button[@class ='header__button ng-star-inserted header__button--active']");
        private readonly By _counterAddedProductsInCart = By.CssSelector("[class='counter counter--green ng-star-inserted']");
        private readonly By _buttonFastMenu = By.Id("fat-menu");
        private readonly By _buttonListFastMenu = By.CssSelector("fat-menu > div > ul > li:nth-child(2)");
        private readonly By _textSumProductsInCart = By.XPath("//span[@class ='cart-receipt__sum-currency']/..");

        private readonly By aaa = By.CssSelector("a.goods-tile__picture.ng-star-inserted > img:nth-child(1)");
        private const int _textSum = 26000;

        [SetUp]
       
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://rozetka.com.ua/");
            driver.Manage().Window.Maximize();
            //driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(200);
        }

        [Test]
        public void AddProductToTheCart()
        {   
            driver.FindElement(_buttonLinkMenuCategoriesNetboock).Click();
            driver.FindElement(_buttonIconFirstListProduct,60).Click();
            driver.FindElement(_buttonBrendProduct,60).Click();
            driver.FindElement(_buttonChoiceByPrice,20).Click();
            driver.FindElement(_buttonChoiceByPriceExpensive).Click();
            WebDriverExtensions.waitforelement(driver, _buttonIconAddProductBay);
            var addProductBay = driver.FindElement(_buttonIconAddProductBay,30);
            addProductBay.Click();

            driver.FindElement(_buttonFastMenu).Click();
            driver.FindElement(_buttonListFastMenu, 20).Click();
            driver.FindElement(_buttonIconSecondListProduct, 80).Click();
            driver.FindElement(_buttonBrendProduct, 20).Click();
            driver.FindElement(_buttonChoiceByPrice).Click();
            driver.FindElement(_buttonChoiceByPriceExpensive).Click();           
            WebDriverExtensions.waitforelement(driver, _buttonIconAddProductBay);
            var addProductBay2 = driver.FindElement(_buttonIconAddProductBay);
            addProductBay2.Click();

            driver.FindElement(_buttonFastMenu).Click();
            driver.FindElement(_buttonListFastMenu).Click();
            driver.FindElement(_buttonIconFirstListProduct, 60).Click();
            driver.FindElement(_buttonBrendProduct).Click();
            driver.FindElement(_buttonChoiceByPrice).Click();
            driver.FindElement(_buttonChoiceByPriceExpensive, 20).Click();      
            WebDriverExtensions.waitforelement(driver, _buttonIconAddProductBay);
            var addProductBay3 = driver.FindElement(_buttonIconAddProductBay, 30);
            addProductBay3.Click();

            Thread.Sleep(300);
            var counterAddedThirdProductsInCart = driver.FindElement(_counterAddedProductsInCart, 30);
            Assert.AreEqual("3", counterAddedThirdProductsInCart.Text, "aren't three items in cart");
            driver.FindElement(_buttonProductsInCart).Click();
            var textSumProductsInCar = driver.FindElement(_textSumProductsInCart, 30).Text;
            Assert.IsTrue(MyAssert.MyIsTrue(_textSum, textSumProductsInCar), "don't incorect sum product in cart");
            Thread.Sleep(3000);
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile(@"C:\Users\Serhii\source\repos\Task2RozetkaUA\Screenshot.png");
       
        }
        [TearDown]
        public void TearDown()
        {
            driver.Close();
        }
    }
}