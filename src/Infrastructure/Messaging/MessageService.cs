using System.Threading.Tasks;
using Domain.Messaging;
using MailKit.Net.Smtp;
using MimeKit;

namespace Infrastructure.Messaging
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class MessageService : IEmailSender, ISmsSender
    {
        private readonly MessageServiceOptions _options;

        public MessageService(MessageServiceOptions options)
        {
            _options = options;
        }

        public async Task<Task> SendEmailAsync(EmailMessageArguments args)
        {        
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(args.FromName, args.FromEmailAddress));

            if (args.AddReplier)
                message.ReplyTo.Add(new MailboxAddress(args.ReplyToName, args.ReplyToEmailAddress));

            message.To.Add(new MailboxAddress(args.RecipientName, args.RecipientEmailAddress));
            message.Subject = args.Subject;

            var bodyBuilder = new BodyBuilder();
            if (args.HasPlainText)
            {
                bodyBuilder.TextBody = args[ContentTypes.Plain];
            }

            if (args.HasHtml)
            {
                bodyBuilder.HtmlBody = args[ContentTypes.Html];
            }

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                var host = _options.HostAddress;
                var port = _options.PortNumber;
                var userName = _options.UserName;
                var password = _options.Password;

                await client.ConnectAsync(host, port);
                await client.AuthenticateAsync(userName, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            return Task.FromResult(0);
        }
    }
}
