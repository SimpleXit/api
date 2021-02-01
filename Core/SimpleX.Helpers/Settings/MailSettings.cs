using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Helpers.Settings
{
    public class MailSettings
    {
        public string Sender { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public SmtpSecurity Security { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public enum SmtpSecurity
    {
        None = 0,
        Auto = 1,
        SslOnConnect = 2,
        StartTls = 3, // (Office365)
        StartTlsWhenAvailable = 4
    }
}
