using RestSharp;
using System.Text.Json;
using System.Net;

using MyTests.Models;

namespace MyTests.Helpers
{
    public class CreateUserApi
    {
        public static HttpStatusCode CreateUser(string username, string password)
        {
            var client = new RestClient("https://demoqa.com/");

            var addUser = new AddUser
            {
                userName = username,
                password = password
            };

            var post = new RestRequest("/Account/v1/User", Method.Post);

            post.AddHeader("Accept", "application/json");
            post.AddJsonBody(addUser);

            var postResp = client.Execute(post);

            Logger.Info($"Создан пользователь: {username} — статус: {postResp.StatusCode}");
            return postResp.StatusCode;
        }
    }
}