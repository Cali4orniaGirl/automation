using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Text.Json;
using MyTests.Models;


namespace MyTests
{
    public class ApiTest
    {
        private int createdBookingId;
        [Test, Order(1)]
        public void CreateBooking_ShouldReturn200AndId()
        {
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

            var request = new RestRequest("/booking", Method.Post);

            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(regBooking);

            var response = client.Execute(request);
            var bookingResponse = JsonSerializer.Deserialize<BookingResponse>(response.Content);

            createdBookingId = bookingResponse.bookingid;
            Assert.That(createdBookingId, Is.GreaterThan(0));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Does.Contain("bookingid"));
        }

        [Test, Order(2)]
        public void GetCreatedBookingId()
        {
            var client = new RestClient("https://restful-booker.herokuapp.com");
            var request = new RestRequest($"/booking/{createdBookingId}", Method.Get);

            request.AddHeader("Accept", "application/json");

            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Does.Contain("Tester"));
        }

        [Test, Order(3)]
        public void DeleteCreatedBooking()
        {
            var client = new RestClient("https://restful-booker.herokuapp.com");

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