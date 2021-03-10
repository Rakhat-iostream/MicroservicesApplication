using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.ApiContainer.Interfaces;
using ShoppingWeb.Models;

namespace ShoppingWeb
{
    public class CheckOutModel : PageModel
    {
        private readonly ICatalogApi _catalogApi;
        private readonly IBasketApi _basketApi;
        private readonly IOrderApi _orderApi;
        private readonly IUserApi _userApi;
        private string userId;

        public CheckOutModel(ICatalogApi catalogApi, IBasketApi basketApi, IUserApi userApi, IOrderApi orderApi)
        {
            _catalogApi = catalogApi ?? throw new ArgumentNullException(nameof(catalogApi));
            _basketApi = basketApi ?? throw new ArgumentNullException(nameof(basketApi));
            _userApi = userApi ?? throw new ArgumentNullException(nameof(_userApi));
            _orderApi = orderApi ?? throw new ArgumentNullException(nameof(_orderApi));
        }

        [BindProperty]
        public OrderResponse Order { get; set; }

        public Basket Cart { get; set; } = new Basket();

        public User User { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            userId =  HttpContext.Session.GetString("userId");
            Cart = await _basketApi.GetBasket(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            User = await _userApi.GetUserById(Guid.Parse(HttpContext.Session.GetString("userId")));
            Cart = await _basketApi.GetBasket(User.Id.ToString());

            if (!ModelState.IsValid)
            {
                return Page();
            }
            Order.Username = User.Id.ToString();
            Order.TotalPrice = Cart.TotalPrice;

            await _orderApi.Checkout(Order);
           // await _basketApi.DeleteCart(Order.Username);

            return RedirectToPage("Confirmation", "OrderSubmitted");
        }
    }
}