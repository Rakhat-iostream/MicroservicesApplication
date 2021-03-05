using ShoppingWeb.ApiContainer.Infrastructure;
using ShoppingWeb.ApiContainer.Interfaces;
using ShoppingWeb.Models;
using ShoppingWeb.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public async Task<IEnumerable<OrderResponse>> GetOrdersByUsername(string username)
        {
            var message = new HttpRequestBuilder(_settings.BaseAddress)
                .SetPath(_settings.OrderingPath)
                .AddQueryString("username", username)
                .HttpMethod(HttpMethod.Get)
                .GetHttpMessage();

            return await base.SendRequest<IEnumerable<OrderResponse>>(message);
        }
    }
}
