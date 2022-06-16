using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace ContactBook.API
{
    public class APITests
    {
        //private const string URL = " http://localhost:8080/api";
        private const string URL = "https://contactbook.nakov.repl.co/api";

        private RestClient client;
        private RestRequest request;


        [SetUp]
        public void Setup()
        {
            this.client = new RestClient(URL);
        }

        [Test]
        public void TestGetAllContacts()
        {
            // arrange
            this.request = new RestRequest(URL + "/contacts");

            // act
            var response = this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts[0].firstName, Is.EqualTo("Steve"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Jobs"));
        }

        [Test]
        public void TestFindContactsKeyword()
        {
            // arrange
            this.request = new RestRequest(URL + "/contacts/search/albert");

            // act
            var response = this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);


            // assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts[0].firstName, Is.EqualTo("Albert"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Einstein"));
        }

        [Test]
        public void TestFindContactsWithMissingKeyword()
        {
            // arrange
            this.request = new RestRequest(URL + "/contacts/search/randnum");

            // act
            var response = this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsEmpty(contacts);
            Assert.That(contacts.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestNewContactsInvalidData()
        {
            // arrange
            this.request = new RestRequest(URL + "/contacts");

            var body = new
            {
                firstName = "Invalid",
                email = "notvalid@abv.bg",
                phone = "not12345678valid"
            };
            request.AddJsonBody(body);

            // act
            var response = this.client.Execute(request, Method.Post);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Last name cannot be empty!\"}"));
        }

        [Test]
        public void TestNewContactsValidData()
        {
            // arrange
            this.request = new RestRequest(URL + "/contacts");

            var body = new
            {
                firstName = "Valid" + DateTime.Now.Ticks,
                lastName = "Valid" + DateTime.Now.Ticks,
                email = DateTime.Now.Ticks + "valid@abv.bg",
                phone = "12345678valid" + DateTime.Now.Ticks
            };
            request.AddJsonBody(body);

            // act
            var response = this.client.Execute(request, Method.Post);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            response = this.client.Execute(request, Method.Get);

            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);
            var lastContact = contacts.Last();

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(lastContact.firstName, Is.EqualTo(body.firstName));
            Assert.That(lastContact.lastName, Is.EqualTo(body.lastName));
        }
    }
}