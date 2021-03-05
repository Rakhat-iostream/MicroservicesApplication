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
        Task<Basket> UpdateBasket(Basket model);
        Task CheckoutBasket(BasketCheckout model);
    }
}
