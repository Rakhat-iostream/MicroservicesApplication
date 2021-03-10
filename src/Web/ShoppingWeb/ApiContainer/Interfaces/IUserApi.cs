using ShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingWeb.ApiContainer.Interfaces
{
    public interface IUserApi
    {
        Task<User> GetUserById(Guid id);
        Task<bool> UpdateUser(User user);
        Task<string> RegistrationToken(User user);
        Task<string> AuthentificationToken(User user);
    }
}
