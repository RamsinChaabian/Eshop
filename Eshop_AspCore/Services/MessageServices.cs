using Eshop_AspCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Eshop_AspCore.Services
{
    public class AuthMessageServices : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            ApplicationDbContext database = new ApplicationDbContext();
            var qservice = database.Tbl_SettingSite.FirstOrDefault();
            
            MailMessage msg = new MailMessage();
            msg.Body = message;
            msg.BodyEncoding = Encoding.UTF8;
            msg.From = new MailAddress(qservice.Email, "فروشگاه اینترنتی", Encoding.UTF8);
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.Normal;
            msg.Sender = msg.From;
            msg.Subject = subject;
            msg.SubjectEncoding = Encoding.UTF8;
            msg.To.Add(new MailAddress(email, "گیرنده", Encoding.UTF8));

            SmtpClient smtp = new SmtpClient();
            smtp.Host = qservice.Smtp;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(qservice.Email, qservice.EmailPwd);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(msg);

            return Task.FromResult(0);

        }
    }
}
