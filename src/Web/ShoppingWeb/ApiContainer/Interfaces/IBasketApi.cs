using ShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingWeb.ApiContainer.Interfaces
{
    public interface IBasketApi
    {
        Task<Basket> GetBasket(string userName);
        Task<bool> DeleteCart(string username);
        Task<Basket> AddItem(string username, BasketItem item);
        Task<bool> DeleteItem(string username, BasketItem item);
        Task CheckoutBasket(OrderResponse model);
    }
}
