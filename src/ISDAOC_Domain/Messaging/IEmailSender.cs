using System.Threading.Tasks;

namespace Domain.Messaging
{
    public interface IEmailSender
    {
        Task<Task> SendEmailAsync(EmailMessageArguments args);
    }
}
