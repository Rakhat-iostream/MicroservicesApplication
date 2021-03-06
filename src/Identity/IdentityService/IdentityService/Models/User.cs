using IdentityService.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models
{
    public class User
    {
        public static readonly string DocumentName = "users";
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsAdmin { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public void SetPassword(string password, IEncryptor encryptor)
        {
            Salt = encryptor.GetSalt(password);
            Password = encryptor.GetHash(password, Salt);
        }
        public bool ValidatePassword(string password, IEncryptor encryptor)
        {
            var isValid = Password.Equals(encryptor.GetHash(password, Salt));
            return isValid;
        }
    }
}
