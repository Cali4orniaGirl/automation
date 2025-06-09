using NUnit.Framework;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework.Internal;
using NUnit.Framework.Interfaces;
using RestSharp;
using System.Net;
using System.Text.Json;

using MyTests.Pages;
using MyTests.Helpers;
using MyTests.Models;


namespace MyTests.Tests
{
    public class Registration
    {
        [Test]
        [TestCase("Testoviy", "Password1!")]
        [Category("API")]
        public void AddUser_isSuccessful(string username, string password)
        {
            var status = CreateUserApi.CreateUser(username, password);
            Assert.That(status, Is.EqualTo(HttpStatusCode.Created));
        }

    }
}



//VladVampire / Password1!