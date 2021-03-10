using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.ApiContainer.Interfaces;
using ShoppingWeb.Models;

namespace ShoppingWeb
{
    public class ProductModel : PageModel
    {
        private readonly ICatalogApi _catalogApi;
        private readonly IBasketApi _basketApi;
        public ProductModel(ICatalogApi catalogApi, IBasketApi basketApi)
        {
            _catalogApi = catalogApi ?? throw new ArgumentNullException(nameof(catalogApi));
            _basketApi = basketApi ?? throw new ArgumentNullException(nameof(basketApi));
        }

        public IEnumerable<string> CategoryList { get; set; } = new List<string>();
        public IEnumerable<Catalog> ProductList { get; set; } = new List<Catalog>();


        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(string categoryName, int pageNumber)
        {
            var productList = await _catalogApi.GetCatalog();

            CategoryList = productList.Select(p => p.Category).Distinct();

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                ProductList = productList.Where(p => p.Category == categoryName);
                SelectedCategory = categoryName;
            }
            else if (pageNumber > 0)
            {
                ProductList = await _catalogApi.GetProductByPage(pageNumber);
            }
            else
            {
                ProductList = productList;
            }
           
            return Page();
        }

        public async Task<IActionResult> OnPostFilteredProductsAsync(string productName, int pageNumber)
        {
            if (!string.IsNullOrEmpty(productName))
            {
                ProductList = await _catalogApi.GetFilteredProducts(productName);
            }
            else
            {
                ProductList = await _catalogApi.GetProductByPage(pageNumber);
            }

            CategoryList = ProductList.Select(p => p.Category).Distinct();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            string userId = HttpContext.Session.GetString("userId");
            if (string.IsNullOrEmpty(userId)) return RedirectToPage("Login", new { loginError = "Please sign in" });
            var product = await _catalogApi.GetProduct(productId);
            var item = new BasketItem
            {
                ProductId = product.Id,
                Price = product.Price,
                ProductName = product.Name,
                Quantity = 1
            };
            await _basketApi.AddItem(userId, item);
            return RedirectToPage("Cart");
        }
    }
}