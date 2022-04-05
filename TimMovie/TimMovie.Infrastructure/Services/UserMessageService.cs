using System.Net.Mail;
using TimMovie.Core.Classes;
using TimMovie.Core.Interfaces;

namespace TimMovie.Infrastructure.Services;

public class UserMessageService : IUserMessageService
{
    public UserMessageService()
    {
        
    }
    public MessageMail GenerateConfirmMessage(string userName, string address, string linkToConfirm)
    {
        return new MessageMail(address,
            $"<p>нихуя не уважаемый {userName}</p><a href='{linkToConfirm}'>вот твоя ссылка уебок</a>", "пошел нахуй");
    }

    public MessageMail GenerateResetPasswordMessage(string userName, string address, string linkToConfirm)
    {
        return new MessageMail(address,
            $"<p>че) {userName} пароль забыл падла?</p> <br> <p>на, уебок {linkToConfirm}</p>", "пошел нахуй");
    }
}