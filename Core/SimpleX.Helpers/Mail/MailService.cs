using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SimpleX.Helpers.Settings;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SimpleX.Helpers.Mail
{
    public interface IMailService
    {
        void Send(SimpleMailMessage message);
        Task SendAsync(SimpleMailMessage message);
    }

    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(
            MailSettings mailSettings)
        {
            _mailSettings = mailSettings ?? 
                throw new ArgumentNullException(nameof(mailSettings));
        }

        public void Send(SimpleMailMessage message)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Connect(_mailSettings.SmtpServer, _mailSettings.Port, (SecureSocketOptions)_mailSettings.Security);
                smtpClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                smtpClient.Send(ConvertMessageToMime(message));
                smtpClient.Disconnect(true);
            }
        }

        public async Task SendAsync(SimpleMailMessage message)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.Port, (SecureSocketOptions)_mailSettings.Security);
                await smtpClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                await smtpClient.SendAsync(ConvertMessageToMime(message));
                await smtpClient.DisconnectAsync(true);
            }
        }

        private MimeMessage ConvertMessageToMime(SimpleMailMessage message)
        {
            ///http://www.mimekit.net/docs/html/Creating-Messages.htm
            var mime = new MimeMessage();

            foreach (string address in message.To.Split(';'))
                mime.To.Add(new MailboxAddress(Encoding.UTF8, address.Trim(), address.Trim()));

            foreach (string address in message.Cc.Split(';'))
                mime.Cc.Add(new MailboxAddress(Encoding.UTF8, address.Trim(), address.Trim()));

            foreach (string address in message.Bcc.Split(';'))
                mime.Bcc.Add(new MailboxAddress(Encoding.UTF8, address.Trim(), address.Trim()));

            mime.From.Add(new MailboxAddress(Encoding.UTF8, _mailSettings.Sender, _mailSettings.Sender));

            mime.Subject = message.Subject;
            
            var builder = new BodyBuilder();
            builder.TextBody = message.BodyText;
            builder.HtmlBody = message.BodyHtml;

            foreach(string path in message.Attachments.Split(';'))
                builder.Attachments.Add(path);

            mime.Body = builder.ToMessageBody();

            return mime;
        }
    }

    public class SimpleMailMessage
    {
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Attachments { get; set; }
        public string BodyText { get; set; }
        public string BodyHtml { get; set; }
    }
}
