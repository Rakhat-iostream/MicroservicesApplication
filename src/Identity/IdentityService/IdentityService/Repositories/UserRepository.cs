using IdentityService.Models;
using IdentityService.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _db;
        public UserRepository(IMongoDatabase db)
        {
            _db = db;
        }
        public User GetUser(string email)
        {
            var col = _db.GetCollection<User>(User.DocumentName);
            var user = col.Find(u => u.Email == email).FirstOrDefault();
            return user;
        }
        public void InsertUser(User user)
        {
            var col = _db.GetCollection<User>(User.DocumentName);
            col.InsertOne(user);
        }

        public User GetUserById(Guid id)
        {   
            var col  = _db.GetCollection<User>(User.DocumentName);
            var user = col.Find(i => i.Id == id).FirstOrDefault();
            return user;
        }
    }
}
