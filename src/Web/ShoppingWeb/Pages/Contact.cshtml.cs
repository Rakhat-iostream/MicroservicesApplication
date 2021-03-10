using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.Models;
using ShoppingWeb.Services;

namespace ShoppingWeb
{
    public class ContactModel : PageModel
    {
        private readonly IMailService _mailSender;

        public ContactModel(IMailService mailSender)
        {
            _mailSender = mailSender;
        }

        public string Welcome { get; set; }
        [BindProperty]
        public Message Message { get; set; }

        public void OnGet()
        {
            Welcome = "Your contact page.";
        }

        public async Task OnPost()
        {
            await _mailSender.Send(Message);
        }
    }
}