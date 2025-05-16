using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Services.Services.Interfaces;

namespace Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(string to, string subject, string body)
        {
            var smtp = new SmtpClient(_config["Smtp:Host"])
            {
                Port = int.Parse(_config["Smtp:Port"]),
                Credentials = new NetworkCredential(_config["Smtp:User"], _config["Smtp:Password"]),
                EnableSsl = true
            };

            var message = new MailMessage(_config["Smtp:From"], to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            smtp.Send(message);
        }

        public void SendConfirmationEmail(string to, string link)
        {
            string body = $"<p>Confirm your email by clicking <a href='{link}'>here</a>.</p>";
            SendEmail(to, "Confirm Your MealPulse Account", body);
        }

        public void SendPasswordResetEmail(string to, string link)
        {
            string body = $"<p>Reset your password: <a href='{link}'>Click here</a>.</p>";
            SendEmail(to, "Reset Your MealPulse Password", body);
        }

        public void SendGoalReminderEmail(string to, string name, int remainingCalories)
        {
            string body = $"<p>Hi {name}, you still have <strong>{remainingCalories} kcal</strong> left today. Keep it up!</p>";
            SendEmail(to, "MealPulse Daily Reminder", body);
        }
    }
}