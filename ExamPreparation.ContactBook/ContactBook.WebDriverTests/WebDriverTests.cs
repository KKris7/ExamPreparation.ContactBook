using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ContactBook.WebDriverSelenium
{
    public class WebDriverTests
    {
        private WebDriver driver;
        //private const string URL = "http://localhost:8080/";
        private const string URL = "https://contactbook.nakov.repl.co/";


        [OneTimeSetUp]
        public void OpenBrowser()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
        }

        [OneTimeTearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }

        [Test]
        public void TestFirstContact()
        {
            // arrange
            driver.Navigate().GoToUrl(URL);
            driver.FindElement(By.LinkText("Contacts")).Click();

            //act
            var firstName = driver.FindElement(By.CssSelector("tr.fname > td")).Text;
            var lastName = driver.FindElement(By.CssSelector("tr.lname > td")).Text;

            // assert
            Assert.That(firstName, Is.EqualTo("Steve"));
            Assert.That(lastName, Is.EqualTo("Jobs"));
        }

        [Test]
        public void TestSearchContact()
        {
            // arrange
            driver.Navigate().GoToUrl(URL);
            driver.FindElement(By.LinkText("Search")).Click();

            //act
            var keywordField = driver.FindElement(By.Id("keyword"));
            keywordField.Click();
            keywordField.SendKeys("albert");
            driver.FindElement(By.Id("search")).Click();
            var firstName = driver.FindElement(By.CssSelector("tr.fname > td")).Text;
            var lastName = driver.FindElement(By.CssSelector("tr.lname > td")).Text;

            // assert
            Assert.That(firstName, Is.EqualTo("Albert"));
            Assert.That(lastName, Is.EqualTo("Einstein"));
        }

        [Test]
        public void TestSearchInvalidData()
        {
            // arrange
            driver.Navigate().GoToUrl(URL);
            driver.FindElement(By.LinkText("Search")).Click();

            //act
            var keywordField = driver.FindElement(By.Id("keyword"));
            keywordField.Click();
            keywordField.SendKeys("invalid2635");
            driver.FindElement(By.Id("search")).Click();
            var searchResult = driver.FindElement(By.CssSelector("#searchResult"));

            // assert
            Assert.That(searchResult.Text, Is.EqualTo("No contacts found."));
        }

        [Test]
        public void TestCreateContactInvalidData()
        {
            // arrange
            driver.Navigate().GoToUrl(URL);
            driver.FindElement(By.LinkText("Create")).Click();

            //act
            var firstNameField = driver.FindElement(By.Id("firstName"));
            firstNameField.Click();
            firstNameField.SendKeys("invalid" + DateTime.Now.Ticks);

            driver.FindElement(By.Id("create")).Click();

            var errorMessage = driver.FindElement(By.CssSelector("body>main>div")).Text;
            
            // assert
            Assert.IsNotEmpty(errorMessage);
        }

        [Test]
        public void TestCreateNewContact()
        {
            // arrange
            driver.Navigate().GoToUrl(URL);
            driver.FindElement(By.LinkText("Create")).Click();
            var firstName = "firstName" + DateTime.Now.Ticks;
            var lastName = "lastName" + DateTime.Now.Ticks;
            var email = "Valid" + DateTime.Now.Ticks + "@abv.bg";
            var phone = "phone" + DateTime.Now.Ticks;


            //act
            var firstNameField = driver.FindElement(By.Id("firstName"));
            firstNameField.SendKeys(firstName);

            var lastNameField = driver.FindElement(By.Id("lastName"));
            lastNameField.SendKeys(lastName);

            var emailField = driver.FindElement(By.Id("email"));
            emailField.SendKeys(email);

            var phoneField = driver.FindElement(By.Id("phone"));
            phoneField.SendKeys(phone);

            driver.FindElement(By.Id("create")).Click();

            var allContacts = driver.FindElements(By.CssSelector("table.contact-entry"));
            var lastContact = allContacts.Last();

            var firstNameNewContact = lastContact.FindElement(By.CssSelector("tr.fname > td")).Text;
            var lastNameNewContact = lastContact.FindElement(By.CssSelector("tr.lname > td")).Text;

            // assert
            Assert.That(firstNameNewContact, Is.EqualTo(firstName));
            Assert.That(lastNameNewContact, Is.EqualTo(lastName));
        }
    }
}