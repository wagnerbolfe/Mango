using System;
using System.Text;
using System.Threading.Tasks;
using EmailAPI.Data;
using EmailAPI.Message;
using EmailAPI.Models;
using EmailAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            var message = new StringBuilder();

            message.AppendLine("<br/>Cart Email Requested ");
            message.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");
            foreach (var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);
        }

        public async Task LogOrderPlaced(RewardsMessage rewardsDto)
        {
            var message = "New Order Placed. <br/> Order ID : " + rewardsDto.OrderId;
            await LogAndEmail(message, "wagnerbolfe@gmail.com");
        }

        public async Task RegisterUserEmailAndLog(string email)
        {
            var message = "User Registration Successful. <br/> Email : " + email;
            await LogAndEmail(message, "wagnerbolfe@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };
                await using var _db = new AppDbContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLog);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
