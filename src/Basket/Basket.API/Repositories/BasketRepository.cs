using Basket.API.Data.Interfaces;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _context;

        public BasketRepository(IBasketContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private async Task<bool> CreateOrUpdateCart(BasketCart cart)
        {
            return await _context.Redis.StringSetAsync(cart.Username, JsonConvert.SerializeObject(cart));
        }

        public async Task<bool> DeleteItem(string username, BasketCartItem item)
        {
            var redisCart = await _context.Redis.StringGetAsync(username);
            if (redisCart.IsNullOrEmpty) return false;
            try
            {
                var cart = JsonConvert.DeserializeObject<BasketCart>(redisCart);
                cart.Items.RemoveAll(i => i.ProductId.Equals(item.ProductId));
                return await CreateOrUpdateCart(cart);
            }
            catch
            {
                return false;
            }
        }

        public async Task<BasketCart> GetCart(string username)
        {
            var redisCart = await _context.Redis.StringGetAsync(username);
            if (redisCart.IsNullOrEmpty) return null;
            try
            {
                return JsonConvert.DeserializeObject<BasketCart>(redisCart);
            }
            catch
            {
                return null;
            }
        }

        public async Task<BasketCart> AddItem(string username, BasketCartItem item)
        {
            var redisCart = await _context.Redis.StringGetAsync(username);
            if (redisCart.IsNullOrEmpty)
            {
                await CreateOrUpdateCart(new BasketCart { Username = username });
                redisCart = await _context.Redis.StringGetAsync(username);
            }
            try
            {
                var cart = JsonConvert.DeserializeObject<BasketCart>(redisCart);
                cart.Items.Add(item);
                if (await CreateOrUpdateCart(cart))
                {
                    return cart;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<BasketCart> UpdateItem(string username, BasketCartItem item)
        {
            var redisCart = await _context.Redis.StringGetAsync(username);
            if (redisCart.IsNullOrEmpty) return null;
            try
            {
                var cart = JsonConvert.DeserializeObject<BasketCart>(redisCart);
                int index = cart.Items.FindIndex(i => i.ProductId.Equals(item.ProductId));
                if (index == -1) return null;
                cart.Items[index] = item;
                if (await CreateOrUpdateCart(cart))
                {
                    return cart;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteCart(string username)
        {
            return await _context.Redis.KeyDeleteAsync(username);
        }
    }
}
