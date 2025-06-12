using System;
using System.Net;
using NUnit.Framework;
using Newtonsoft.Json; 
using RestSharp;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Helpers;
using Newtonsoft.Json.Linq;

namespace ApiTests.steps
{
    [Binding]
    public class BookingApiSteps
    {
        private RestClient _client;
        private RestResponse _lastResponse;
        private int _bookingId;
        private int _expectedPrice;

        [Given(@"base URL ""(.*)""")]
        public void GivenBaseUrl(string url)
        {
            _client = new RestClient(url);
            _client.AddDefaultHeader("Accept", "application/json");
        }

        [When(@"I create a booking with:")]
        public void WhenICreateBookingWith(Table table)
        {
            var row = table.Rows[0];
            var req = new RestRequest("/booking", Method.Post)
                .AddHeader("Accept", "application/json")
                .AddJsonBody(new
                {
                    firstname = row["FirstName"],
                    lastname = row["LastName"],
                    totalprice = int.Parse(row["TotalPrice"]),
                    depositpaid = bool.Parse(row["DepositPaid"]),
                    bookingdates = new
                    {
                        checkin = row["Checkin"],
                        checkout = row["Checkout"]
                    },
                    additionalneeds = row["AdditionalNeeds"]
                });

            _lastResponse = _client.Execute(req);
            Logger.Info($"[API CREATE] Status={(int)_lastResponse.StatusCode} Body=<<{_lastResponse.Content}>>");

            Assert.That(_lastResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK),
                $"Couldn't create booking: {_lastResponse.Content}");

            var json = JObject.Parse(_lastResponse.Content);
            _bookingId = json.Value<int>("bookingid");
            _expectedPrice = json["booking"].Value<int>("totalprice");
        }

        [Then(@"the API response status is (.*)")]
        public void ThenTheApiResponseStatusIs(int code)
        {
            Assert.That(_lastResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        [Then(@"the booking can be retrieved with price (.*)")]
        public void ThenTheBookingCanBeRetrievedWithPrice(int expected)
        {
            var getReq = new RestRequest($"/booking/{_bookingId}", Method.Get)
                .AddHeader("Accept", "application/json");
            var getResp = _client.Execute(getReq);

            Assert.That(getResp.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Console.WriteLine("Response GET /booking/: " + getResp.Content);

            dynamic body = JsonConvert.DeserializeObject<dynamic>(getResp.Content);
            Assert.That((int)body.totalprice, Is.EqualTo(_expectedPrice));
        }
    }
}