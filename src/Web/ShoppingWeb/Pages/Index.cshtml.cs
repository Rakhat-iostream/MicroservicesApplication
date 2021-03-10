using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.ApiContainer.Interfaces;
using ShoppingWeb.Models;

namespace ShoppingWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICatalogApi _catalogApi;
        private readonly IBasketApi _basketApi;
        private string userId;

        public IndexModel(ICatalogApi catalogApi, IBasketApi basketApi)
        {
            _catalogApi = catalogApi ?? throw new ArgumentNullException(nameof(catalogApi));
            _basketApi = basketApi ?? throw new ArgumentNullException(nameof(basketApi));
        }

        public IEnumerable<Catalog> ProductList { get; set; } = new List<Catalog>();

        public async Task<IActionResult> OnGetAsync()
        {
            ProductList = await _catalogApi.GetCatalog();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });

            userId = HttpContext.Session.GetString("userId");
            if (string.IsNullOrEmpty(userId)) return RedirectToPage("Login", new { loginError = "Please sign in" });
            var item = await _catalogApi.GetProduct(productId);
            await _basketApi.AddItem(userId, new BasketItem
            {
                ProductId = productId,
                Color = "Black",
                Price = item.Price,
                Quantity = 1,
                ProductName = item.Name
            });
            return RedirectToPage("Cart");
        }
    }
}
