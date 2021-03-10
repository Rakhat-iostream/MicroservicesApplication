using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.ApiContainer.Interfaces;
using ShoppingWeb.DTOs;
using ShoppingWeb.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingWeb.Pages
{
    public class RegisterModel : PageModel
    {
        private IUserApi _userApi;
        public RegisterModel(IUserApi userApi)
        {
            _userApi = userApi ?? throw new ArgumentNullException(nameof(_userApi));
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostRegisterUserAsync(RegisterDto regUser)
        {
            if (!ModelState.IsValid) return RedirectToPage(new { registrationError = "Fill all fields" });
            User user = new User
            {
                Firstname = regUser.Firstname,
                Lastname = regUser.Lastname,
                Password = regUser.Password,
                Username = regUser.Username,
                Email = regUser.Email,
                /*Role = regUser.Role,*/
                Id = Guid.NewGuid()
            };
            string responseString = await _userApi.RegistrationToken(user);
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
                    var errorResponse = new { registrationError = responseString };
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
