using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.ApiContainer.Interfaces;
using ShoppingWeb.Models;

namespace ShoppingWeb
{
    public class CartModel : PageModel
    {
        private readonly IBasketApi _basketApi;
        private string userId;

        public CartModel(IBasketApi basketApi)
        {
            _basketApi = basketApi ?? throw new ArgumentNullException(nameof(basketApi));
        }

        public Basket Cart { get; set; } = new Basket();

        public async Task<IActionResult> OnGetAsync()
        {
            userId = HttpContext.Session.GetString("userId");
            Cart = await _basketApi.GetBasket(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string productId)
        {
          
            userId = HttpContext.Session.GetString("userId");
            Cart = await _basketApi.GetBasket(userId);
            await _basketApi.DeleteItem(Cart.Username, Cart.Items.Find(i => i.ProductId == productId));
            return RedirectToPage();
        }
    }
}