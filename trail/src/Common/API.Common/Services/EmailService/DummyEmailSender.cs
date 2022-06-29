using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ID.eShop.API.Common.Services
{
    public class DummyEmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public DummyEmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(Message message)
        {
            System.Diagnostics.Debug.WriteLine($"------------- {nameof(DummyEmailSender)} ------------");
            System.Diagnostics.Debug.WriteLine(message.ToString());
        }

        public Task SendEmailAsync(Message message)
        {
            System.Diagnostics.Debug.WriteLine($"------------- {nameof(DummyEmailSender)} ------------");
            System.Diagnostics.Debug.WriteLine(message.ToString());

            return Task.CompletedTask;
        }
    }
}
