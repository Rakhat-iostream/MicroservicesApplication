using Newtonsoft.Json;
using ShoppingWeb.ApiContainer.Infrastructure;
using ShoppingWeb.ApiContainer.Interfaces;
using ShoppingWeb.Models;
using ShoppingWeb.Settings;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingWeb.ApiContainer
{
    public class UserApi : BaseHttpClientWithFactory, IUserApi
    {
        private readonly IApiSettings _settings;
        private readonly HttpRequestBuilder _builder;

        public UserApi(IApiSettings settings, IHttpClientFactory factory) : base(factory)
        {
            _settings = settings;
            _builder = new HttpRequestBuilder(_settings.BaseAddress);
            _builder.AddToPath(_settings.UserPath);
        }

        public async Task<User> GetUserById(Guid id)
        {
            using var message = _builder
            .HttpMethod(HttpMethod.Get).AddToPath(id.ToString())
            .GetHttpMessage();
            return await GetResponseAsync<User>(message);
        }

        public async Task<string> AuthentificationToken(User user)
        {
            using var message = new HttpRequestBuilder(_settings.BaseAddress + _settings.UserPath)
            .HttpMethod(HttpMethod.Post).AddToPath("/login").
            Content(new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"))
            .GetHttpMessage();
            return await GetResponseStringAsync(message);
        }

        public async Task<string> RegistrationToken(User user)
        {
            using var message = new HttpRequestBuilder(_settings.BaseAddress + _settings.UserPath)
            .HttpMethod(HttpMethod.Post).AddToPath("/register").
            Content(new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"))
            .GetHttpMessage();
            return await GetResponseStringAsync(message);
        }

        public async Task<bool> UpdateUser(User user)
        {
              _builder.AddToPath(_settings.UserPath);
             using var message = _builder
            .HttpMethod(HttpMethod.Put).AddToPath("/" + user.Id).
            Content(new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"))
            .GetHttpMessage();
                return await GetResponseStringAsync(message) != null;
            
        }

        public override async Task<string> GetResponseStringAsync(HttpRequestMessage request)
        {
            using var client = GetHttpClient();
            using var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
  