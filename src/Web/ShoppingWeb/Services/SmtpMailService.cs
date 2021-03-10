using ShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ShoppingWeb.Services
{
    public class SmtpMailService : IMailService
    {
        public async Task Send(Message message)
        {
            using (var smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtp.PickupDirectoryLocation = @"D:\maildump";
                var msg = new MailMessage
                {
                    Body = message.Body,
                    Subject = message.Subject,
                    From = new MailAddress(message.From)
                };
                msg.To.Add(new MailAddress("iskrobraz@mail.ru"));
                await smtp.SendMailAsync(msg);
            }
        }
    }
}
