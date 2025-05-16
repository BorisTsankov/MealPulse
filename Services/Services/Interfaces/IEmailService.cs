using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
        void SendConfirmationEmail(string to, string confirmationLink);
        void SendPasswordResetEmail(string to, string resetLink);
        void SendGoalReminderEmail(string to, string userName, int remainingCalories);
    }
}