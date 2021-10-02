using System.Threading.Tasks;

namespace Domain.Messaging
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
