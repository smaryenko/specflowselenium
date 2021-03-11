namespace specflowselenium.Bindings
{
    using System;
    using System.Collections.Generic;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using OpenQA.Selenium.Chrome;
    using TechTalk.SpecFlow;
    using NUnit.Framework;

    using static SeleniumExtras.WaitHelpers.ExpectedConditions;

    [Binding]
    class SpecflowBindings
    {
        private static IWebDriver _driver;
        private static WebDriverWait _waitDriver;

        [When(@"I start the browser")]
        public void WhenIStartTheBrowser()
        {
            _driver = new ChromeDriver();
            _waitDriver = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        }

        [When(@"I navigate to '(.*)'")]
        public void WhenINavigateToHttpExample_Com(string Url)
        {
            _driver.Navigate().GoToUrl(Url);
        }

        [When(@"I click on the '(.*)'")]
        public void ClickOnLinkByText(string linkText)
        {
            _waitDriver.Until(ElementToBeClickable(By.LinkText(linkText))).Click();
        }

        [Then(@"a link with text '(.*)' must be present")]
        public void WaitForLinkWithText(string linkText)
        {
            _waitDriver.Until(ElementIsVisible(By.LinkText(linkText)));
        }

        [Then(@"the '(.*)' box must contain '(.*)' at index '(.*)'")]
        public void WaitForElementToBeAtPosition(string rootText, string nestedText, int position)
        {
            By parentXpath = By.XPath($"//h2[contains(text(),'{rootText}')]/parent::div");
            By childXpath = By.XPath("ul/li/a");

            IWebElement rootElement = _waitDriver.Until(ElementIsVisible(parentXpath));
            IList<IWebElement> nestedElements = rootElement.FindElements(childXpath);

            Assert.AreEqual(nestedText, nestedElements[position - 1].Text); 
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            _driver.Close();
            _driver.Quit();
        }
    }
}
