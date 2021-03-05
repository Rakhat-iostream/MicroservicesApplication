using ShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingWeb.ApiContainer.Interfaces
{
    public interface IOrderApi
    {
        Task<IEnumerable<OrderResponse>> GetOrdersByUsername(string username);
    }
}
