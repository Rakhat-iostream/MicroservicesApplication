﻿using Newtonsoft.Json;
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
    public class CatalogApi : BaseHttpClientWithFactory, ICatalogApi
    {
        private readonly IApiSettings _settings;

        public CatalogApi(IHttpClientFactory factory, IApiSettings settings)
            : base(factory)
        {
            _settings = settings;
        }

        public async Task<IEnumerable<Catalog>> GetCatalog()
        {
            var message = new HttpRequestBuilder(_settings.BaseAddress)
                               .SetPath(_settings.CatalogPath)
                               .HttpMethod(HttpMethod.Get)
                               .GetHttpMessage();

            return await SendRequest<IEnumerable<Catalog>>(message);
        }

        public async Task<IEnumerable<Catalog>> GetFilteredProducts(string productName)
        {
            using var message = new HttpRequestBuilder(_settings.BaseAddress).AddQueryString("productName", productName)
              .HttpMethod(HttpMethod.Get)
              .GetHttpMessage();
            return await GetResponseAsync<IEnumerable<Catalog>>(message);
        }
      

        public async Task<IEnumerable<Catalog>> GetCatalogByCategory(string category)
        {
            var message = new HttpRequestBuilder(_settings.BaseAddress)
                               .SetPath(_settings.CatalogPath)
                               .AddToPath("GetProductByCategory")
                               .AddToPath(category)
                               .HttpMethod(HttpMethod.Get)
                               .GetHttpMessage();

            return await SendRequest<IEnumerable<Catalog>>(message);
        }

        public async Task<Catalog> CreateCatalog(Catalog catalogModel)
        {
            var message = new HttpRequestBuilder(_settings.BaseAddress)
                                .SetPath(_settings.CatalogPath)
                                .HttpMethod(HttpMethod.Post)
                                .GetHttpMessage();

            var json = JsonConvert.SerializeObject(catalogModel);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return await SendRequest<Catalog>(message);
        }

        public async Task<Catalog> GetProduct(string id)
        {
            using var message = new HttpRequestBuilder(_settings.BaseAddress + _settings.CatalogPath).AddToPath(id)
           .HttpMethod(HttpMethod.Get)
           .GetHttpMessage();
            return await GetResponseAsync<Catalog>(message);
        }

        public async Task<IEnumerable<Catalog>> GetProductByCategory(string categoryName)
        {
            using var message = new HttpRequestBuilder(_settings.BaseAddress + _settings.CatalogPath).AddToPath("GetProductsByCategory/" + categoryName)
              .HttpMethod(HttpMethod.Get)
              .GetHttpMessage();
            return await GetResponseAsync<IEnumerable<Catalog>>(message);
        }

       

        public async Task<IEnumerable<Catalog>> GetProductByPage(int page)
        {

            var _builder = new HttpRequestBuilder(_settings.BaseAddress).AddToPath(_settings.CatalogPath);
            
                var message = _builder.AddQueryString("page", page.ToString())
                .HttpMethod(HttpMethod.Get)
                .GetHttpMessage();
            return await GetResponseAsync<IEnumerable<Catalog>>(message);
        }
    }
}

