using IdentityService.Models;
using IdentityService.Repositories;
using IdentityService.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtBuilder _jwtBuilder;
        private readonly IEncryptor _encryptor;
        public UserController
               (IMongoDatabase db, IJwtBuilder jwtBuilder, IEncryptor encryptor)
        {
            _userRepository = new UserRepository(db);
            _jwtBuilder = jwtBuilder;
            _encryptor = encryptor;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] User user,
        [FromQuery(Name = "d")] string destination = "frontend")
        {
            var u = _userRepository.GetUser(user.Email);
            if (u == null)
            {
                return NotFound("User not found.");
            }
            if (destination == "backend" && !u.IsAdmin)
            {
                return BadRequest("Could not authenticate user.");
            }
            var isValid = u.ValidatePassword(user.Password, _encryptor);
            if (!isValid)
            {
                return BadRequest("Could not authenticate user.");
            }
            var token = _jwtBuilder.GetToken(u.Id);
            return new OkObjectResult(token);
        }

        [HttpGet("{id}")]
        public User GetUserById(Guid id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null) return null;
            return user;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            var u = _userRepository.GetUser(user.Email);
            if (u != null)
            {
                return BadRequest("User already exists.");
            }
            user.SetPassword(user.Password, _encryptor);
            
            _userRepository.InsertUser(user);
            user = _userRepository.GetUser(user.Email);
            var token = _jwtBuilder.GetToken(user.Id);
            return new OkObjectResult(token);
    
        }
        [HttpGet("validate")]
        public IActionResult Validate([FromQuery(Name = "email")] string email,
        [FromQuery(Name = "token")] string token)
        {
            var u = _userRepository.GetUser(email);
            if (u == null)
            {
                return NotFound("User not found.");
            }
            var userId = _jwtBuilder.ValidateToken(token);
            if (userId != u.Id)
            {
                return BadRequest("Invalid token.");
            }
            return new OkObjectResult(userId);
        }
    }
}