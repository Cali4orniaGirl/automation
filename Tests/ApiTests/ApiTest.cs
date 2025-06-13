using RestSharp;
using System.Net;
using System.Text.Json;
using Allure.NUnit.Attributes;
using Allure.NUnit;

using Models;

namespace Automation.Tests.ApiTests
{
    [TestFixture]
    [AllureSuite("Booking API")]
    [AllureNUnit]
    

    public class ApiTest
    {
        public static IEnumerable<TestCaseData> BookingTestingData()
        {
            yield return new TestCaseData("Anna", "Bell");
            yield return new TestCaseData("Lama", "Bell");
        }

        [AllureFeature("CreateGetDeleteBooking")]
        [AllureStory("Created, verified and deleted booking request")]
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.normal)]
        [Test]
        [TestCaseSource(nameof(BookingTestingData))]
        [Category("API")]
        public void CreateGetDeleteBookingRequest(string firstname, string lastname)
        {
            // Specifiying API endpoints
            var client = new RestClient("https://restful-booker.herokuapp.com");

            var regBooking = new BookingRequest
            {
                firstname = firstname,
                lastname = lastname,
                totalprice = 100,
                depositpaid = true,
                bookingdates = new BookingDates
                {
                    checkin = "2025-01-01",
                    checkout = "2026-01-01"
                },
                additionalneeds = "dinner"

            };
            // Creating booking request
            var post = new RestRequest("/booking", Method.Post);

            post.AddHeader("Accept", "application/json");
            post.AddJsonBody(regBooking);

            var postResp = client.Execute(post);
            var bookingResponse = JsonSerializer.Deserialize<BookingResponse>(postResp.Content);

            var createdBookingId = bookingResponse.bookingid;
            Assert.That(postResp.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(postResp.Content, Does.Contain("bookingid"));

            var get = new RestRequest($"/booking/{createdBookingId}", Method.Get);
            // Getting created booking request
            get.AddHeader("Accept", "application/json");

            var getResp = client.Execute(get);
            Console.WriteLine($"The ID: {createdBookingId}");

            Assert.That(getResp.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getResp.Content, Does.Contain("Bell"));

            // Admin auth + deleting created booking request
            var authRequest = new RestRequest("/auth", Method.Post);
            authRequest.AddJsonBody(new AuthRequest { username = "admin", password = "password123" });

            var authResponse = client.Execute(authRequest);
            var token = JsonSerializer.Deserialize<AuthResponse>(authResponse.Content).token;

            var deleteRequest = new RestRequest($"/booking/{createdBookingId}", Method.Delete);
            deleteRequest.AddHeader("Cookie", $"token={token}");

            var deleteResponse = client.Execute(deleteRequest);

            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

    }
}