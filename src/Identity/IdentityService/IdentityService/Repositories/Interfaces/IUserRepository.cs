using IdentityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(string email);
        void InsertUser(User user);
        User GetUserById(Guid id);
    }
}
