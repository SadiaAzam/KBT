using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace KBT.Services
{
    public class EmailSender
    {
        public EmailSender()
        {

        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string fromMail = "sadirajput113@gmail.com";
            string fromPassword = "Rana@123456789";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body> " + htmlMessage + " </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };
            smtpClient.Send(message);
        }
    }
}
