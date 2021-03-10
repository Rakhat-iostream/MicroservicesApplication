using Newtonsoft.Json;
using ShoppingWeb.ApiContainer.Infrastructure;
using ShoppingWeb.ApiContainer.Interfaces;
using ShoppingWeb.Models;
using ShoppingWeb.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingWeb.ApiContainer
{
    public class BasketApi : BaseHttpClientWithFactory, IBasketApi
    {
        private readonly IApiSettings _settings;

        public BasketApi(IHttpClientFactory factory, IApiSettings settings)
            : base(factory)
        {
            _settings = settings;
        }

        public async Task<Basket> GetBasket(string username)
        {
            using var message = new HttpRequestBuilder(_settings.BaseAddress + _settings.BasketPath).AddQueryString("username", username)
            .HttpMethod(HttpMethod.Get)
            .GetHttpMessage();
            return await GetResponseAsync<Basket>(message);
        }

        public async Task<Basket> AddItem(string username, BasketItem item)
        {
            using var message = new HttpRequestBuilder(_settings.BaseAddress + _settings.BasketPath)
            .HttpMethod(HttpMethod.Post).AddQueryString("username", username).
            Content(new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"))
            .GetHttpMessage();
            return await GetResponseAsync<Basket>(message);
        }

        public async Task<bool> DeleteCart(string username)
        {

            var _builder = new HttpRequestBuilder(_settings.BaseAddress).AddToPath(_settings.BasketPath);

            using var message = _builder.AddToPath(username)
            .HttpMethod(HttpMethod.Delete)
            .GetHttpMessage();
            var response = await GetResponseStringAsync(message);
            return response != null;
        }


        public async Task<bool> DeleteItem(string username, BasketItem item)
        {

            var _builder = new HttpRequestBuilder(_settings.BaseAddress).AddToPath(_settings.BasketPath);

            using var message = _builder.
                AddQueryString("username", username).
            Content(new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"))
            .HttpMethod(HttpMethod.Delete)
            .GetHttpMessage();
            var response = await GetResponseStringAsync(message);
            return response != null;
        }


        public async Task CheckoutBasket(OrderResponse model)
        {
            var message = new HttpRequestBuilder(_settings.BaseAddress)
                                .SetPath(_settings.BasketPath)
                                .AddToPath("Checkout")
                                .HttpMethod(HttpMethod.Post)
                                .GetHttpMessage();

            var json = JsonConvert.SerializeObject(model);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");

            await SendRequest<OrderResponse>(message);
        }
    }
}


