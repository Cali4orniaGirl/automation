using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Text.Json;
using MyTests.Models;


namespace MyTests
{
    public class ApiTest
    {

        [Test]
        [Category("API")]
        public void CreateGetDeleteBookingRequest()
        {
            // Specifiying API endpoints
            var client = new RestClient("https://restful-booker.herokuapp.com");

            var regBooking = new BookingRequest
            {
                firstname = "Tester",
                lastname = "Portfolio",
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
            Assert.That(getResp.Content, Does.Contain("Tester"));

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