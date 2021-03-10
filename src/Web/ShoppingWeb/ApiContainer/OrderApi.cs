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
    public class OrderApi : BaseHttpClientWithFactory, IOrderApi
    {
        private readonly IApiSettings _settings;

        public OrderApi(IHttpClientFactory factory, IApiSettings settings) : base(factory)
        {
            _settings = settings;
        }

        public async Task Checkout(OrderResponse order)
        {
            using var message = new HttpRequestBuilder(_settings.BaseAddress).SetPath(_settings.BasketPath).AddToPath("/Checkout").
                Content(new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json"))
            .HttpMethod(HttpMethod.Post)
            .GetHttpMessage();
            await GetResponseStringAsync(message);
        }

        public async Task<bool> DeleteOrderById(int id)
        {
            using var message = new HttpRequestBuilder(_settings.BaseAddress).SetPath(_settings.OrderingPath).AddToPath("/" + id)
            .HttpMethod(HttpMethod.Delete)
            .GetHttpMessage();
            return await GetResponseStringAsync(message) != null;
        }

        public async Task<OrderResponse> GetOrderById(int id)
        {
            using var message = new HttpRequestBuilder(_settings.BaseAddress).SetPath(_settings.OrderingPath).AddToPath("/" + id)
            .HttpMethod(HttpMethod.Get)
            .GetHttpMessage();
            return await GetResponseAsync<OrderResponse>(message);
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersByUsername(string username)
        {
            using var message = new HttpRequestBuilder(_settings.BaseAddress).SetPath(_settings.OrderingPath).AddQueryString("username", username)
            .HttpMethod(HttpMethod.Get)
            .GetHttpMessage();
            return await GetResponseAsync<IEnumerable<OrderResponse>>(message);
        }
    }
}
