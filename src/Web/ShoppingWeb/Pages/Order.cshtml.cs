using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.ApiContainer.Interfaces;
using ShoppingWeb.Models;

namespace ShoppingWeb
{
    public class OrderModel : PageModel
    {
        private readonly IOrderApi _orderApi;
        private string userId;

        public OrderModel(IOrderApi orderApi)
        {
            _orderApi = orderApi;
        }

        public IEnumerable<OrderResponse> Orders { get; set; } = new List<OrderResponse>();

        public async Task<IActionResult> OnGetAsync()
        {
            userId = HttpContext.Session.GetString("userId");
            Orders = await _orderApi.GetOrdersByUsername(userId);
            return Page();
        }       
    }
}