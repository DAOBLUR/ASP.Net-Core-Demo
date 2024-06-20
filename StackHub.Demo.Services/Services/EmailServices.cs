using System.Net.Mail;
using System.Net;

namespace StackHub.Demo.Services.Services
{
    public class EmailServices
    {
        public static void WelcomeEmail(string toEmail, string displayName)
        {
            var fromAddress = new MailAddress("daoblur.business@gmail.com", "StackHub");
            var toAddress = new MailAddress(toEmail, displayName);

            const string fromPassword = "tsckluvcqvnncwnl";
            const string subject = "Welcome";
            string body = $"Subject: Welcome to StackHub!\r\n\r\nDear {displayName},\r\n\r\nWe're thrilled to have you on board! Welcome to StackHub, where we strive to offer you the best experience possible.\r\n\r\nGetting started is easy, and there are plenty of resources available to help you make the most out of our service. If you have any questions or need assistance, our support team is here for you.\r\n\r\nThank you for choosing us. We're looking forward to serving you.\r\n\r\nBest regards,\r\nThe StackHub Team\r\n ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            smtp.Send(message);
        }
    }
}
