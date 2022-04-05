using TimMovie.Core.Classes;

namespace TimMovie.Core.Interfaces;

public interface IMailService
{
    public Task SendMessageAsync(MessageMail message);
}