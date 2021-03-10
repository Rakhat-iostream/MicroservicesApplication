using ShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingWeb.Services
{
    public interface IMailService
    {
        Task Send(Message message);
    }
}
