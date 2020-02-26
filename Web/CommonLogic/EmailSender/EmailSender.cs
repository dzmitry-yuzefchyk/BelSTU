using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CommonLogic.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private readonly int _port;
        private readonly bool _isSSLEnabled;

        public EmailSender(string hostName, string userName, string password,
            int port, bool isSSLEnabled)
        {
            _hostName = hostName;
            _userName = userName;
            _password = password;
            _port = port;
            _isSSLEnabled = isSSLEnabled;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_hostName, _port)
            {
                Credentials = new NetworkCredential(_userName, _password),
                EnableSsl = _isSSLEnabled
            };
            var message = new MailMessage(_userName, email, subject, htmlMessage) { IsBodyHtml = true };
            return client.SendMailAsync(message);
        }
    }
}
