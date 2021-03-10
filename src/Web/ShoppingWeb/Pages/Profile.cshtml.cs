using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.ApiContainer.Interfaces;
using ShoppingWeb.Models;

namespace ShoppingWeb.Pages
{
    public class ProfileModel : PageModel
    {
        private IUserApi _userApi;
        public ProfileModel(IUserApi userApi)
        {
            _userApi = userApi ?? throw new ArgumentNullException(nameof(_userApi));
        }
        public User User { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            User = await _userApi.GetUserById(Guid.Parse(HttpContext.Session.GetString("userId")));
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateUserAsync(User updateUser)
        {
            if (updateUser.Id.Equals(Guid.Empty))
            {
                ViewData["updateUserError"] = "Id is not specified";
                return Page();
            }
            User user = await _userApi.GetUserById(updateUser.Id);
            UpdateUserValues(user, updateUser);
            if (await _userApi.UpdateUser(user))
            {
                return RedirectToPage();
            }
            ViewData["updateUserError"] = "Failed to update user";
            return Page();
        }

        public IActionResult OnPostSignout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("Index");
        }


        private void UpdateUserValues(User user, User updateUser)
        {
            if (!string.IsNullOrEmpty(updateUser.Email)) user.Email = updateUser.Email;
            if (!string.IsNullOrEmpty(updateUser.Username)) user.Username = updateUser.Username;
            if (!string.IsNullOrEmpty(updateUser.Password)) user.Password = updateUser.Password;
        }
    }
}
