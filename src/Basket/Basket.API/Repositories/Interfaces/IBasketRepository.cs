using Basket.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<BasketCart> GetCart(string username);
        Task<bool> DeleteCart(string username);
        Task<BasketCart> AddItem(string username, BasketCartItem item);
        Task<BasketCart> UpdateItem(string username, BasketCartItem item);
        Task<bool> DeleteItem(string username, BasketCartItem item);
    }
}
