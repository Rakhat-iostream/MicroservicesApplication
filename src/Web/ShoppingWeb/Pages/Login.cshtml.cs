using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.ApiContainer.Interfaces;
using ShoppingWeb.DTOs;
using ShoppingWeb.Models;

namespace ShoppingWeb.Pages
{
    public class LoginModel : PageModel
    {
        private IUserApi _userApi;
        public LoginModel(IUserApi userApi)
        {
            _userApi = userApi ?? throw new ArgumentNullException(nameof(_userApi));
        }



        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostLoginUserAsync(LoginDto loginUser)
        {
            if (!ModelState.IsValid) return RedirectToPage(new { loginError = "Fill all fields" });
            User user = new User
            {
                Password = loginUser.Password,
                Email = loginUser.Email,
            };
            string responseString = await _userApi.AuthentificationToken(user);
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var claims = handler.ReadJwtToken(responseString).Claims;
                string id = claims.FirstOrDefault(claim => claim.Type == "userId").Value;
                if (!string.IsNullOrEmpty(id)) HttpContext.Session.SetString("userId", id);
            }
            catch (Exception e)
            {
                if (responseString.Length < 50)
                {
                    var errorResponse = new { loginError = responseString };
                    return RedirectToPage(errorResponse);
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("Product", new { pageNumber = 1 });
        }
    }
}