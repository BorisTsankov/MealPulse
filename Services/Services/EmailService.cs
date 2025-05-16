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
            string subject = "Confirm Your MealPulse Account";

            string body = $@"
    <html>
    <head>
        <style>
            {GetEmailCss()}
        </style>
    </head>
    <body>
        <div class='email-container'>
            {GetEmailHeader()}
            <h2>Welcome to MealPulse!</h2>
            <p>Thanks for joining MealPulse. Please confirm your email address to activate your account:</p>
            <p style='text-align:center;'>
                <a href='{link}' class='button'>Confirm Email</a>
            </p>
            <p>If you didn’t sign up, you can safely ignore this email.</p>
            {GetEmailFooter()}
        </div>
    </body>
    </html>";

            SendEmail(to, subject, body);
        }


        public void SendPasswordResetEmail(string to, string link)
        {
            string subject = "Reset Your MealPulse Password";

            string logoBase64 = Convert.ToBase64String(File.ReadAllBytes("wwwroot/images/Clean_logo.png")); // your uploaded logo
            string body = $@"
    <html>
    <head>
        <style>
            @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;600&display=swap');

            body {{
                font-family: 'Inter', sans-serif;
                background-color: #f5f5f5;
                margin: 0;
                padding: 20px;
            }}
            .email-wrapper {{
                max-width: 600px;
                margin: auto;
                background-color: #ffffff;
                border-radius: 12px;
                overflow: hidden;
                box-shadow: 0 0 10px rgba(0,0,0,0.05);
                padding: 30px;
            }}
            .logo {{
                text-align: center;
                margin-bottom: 30px;
            }}
            .logo img {{
                height: 50px;
            }}
            h2 {{
                color: #333;
                font-size: 22px;
                margin-bottom: 16px;
            }}
            p {{
                color: #555;
                font-size: 16px;
                line-height: 1.5;
            }}
            .button {{
                display: inline-block;
                background-color: #6f42c1;
                color: #ffffff !important;
                text-decoration: none;
                padding: 12px 24px;
                font-weight: 600;
                border-radius: 6px;
                margin-top: 20px;
            }}
            .footer {{
                text-align: center;
                font-size: 12px;
                color: #aaa;
                margin-top: 40px;
            }}
        </style>
    </head>
    <body>
        <div class='email-wrapper'>
            <div class='logo'>
                <img src='data:image/png;base64,{logoBase64}' alt='MealPulse Logo' />
            </div>
            <h2>Password Reset Request</h2>
            <p>We received a request to reset your password. Click the button below to choose a new one:</p>
            <div style='text-align:center;'>
                <a href='{link}' class='button'>Reset Password</a>
            </div>
            <p>This link will expire in 1 hour. If you didn't request a password reset, you can safely ignore this email.</p>
            <div class='footer'>
                &copy; {DateTime.Now.Year} MealPulse. All rights reserved.
            </div>
        </div>
    </body>
    </html>";

            SendEmail(to, subject, body);
        }



        public void SendGoalReminderEmail(string to, string name, int remainingCalories)
        {
            string body = $"<p>Hi {name}, you still have <strong>{remainingCalories} kcal</strong> left today. Keep it up!</p>";
            SendEmail(to, "MealPulse Daily Reminder", body);
        }

        private string GetEmailCss()
        {
            return @"
        .email-container {
            font-family: Arial, sans-serif;
            max-width: 600px;
            margin: auto;
            padding: 20px;
            border: 1px solid #eee;
            background-color: #f9f9f9;
            color: #333;
        }
        .button {
            display: inline-block;
            background-color: #6f42c1;
            color: white;
            padding: 12px 20px;
            text-decoration: none;
            border-radius: 6px;
            font-weight: bold;
        }
        .footer {
            margin-top: 30px;
            font-size: 12px;
            text-align: center;
            color: #888;
        }
        .logo {
            text-align: center;
            margin-bottom: 20px;
        }";
        }

        private string GetEmailHeader()
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Clean_logo.png");
            string base64Image = Convert.ToBase64String(File.ReadAllBytes(imagePath));
            string imgTag = $"<div class='logo'><img src='data:image/png;base64,{base64Image}' alt='MealPulse Logo' height='50'/></div>";
            return imgTag;
        }


        private string GetEmailFooter()
        {
            return $@"<div class='footer'>
                &copy; {DateTime.Now.Year} MealPulse. All rights reserved.
              </div>";
        }

    }
}