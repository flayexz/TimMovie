using TimMovie.Core.Classes;
using TimMovie.Core.Interfaces;
using TimMovie.Infrastructure.MailTemplates;

namespace TimMovie.Infrastructure.Services;

public class UserMessageService : IUserMessageService
{
    public UserMessageService()
    {
    }

    public MessageMail GenerateConfirmMessage(string userName, string linkToConfirm)
    {
        var header = $"Салам алейкум, {userName}!";
        var content =
            $"<span style=\"font-family:Arial,sans-serif;font-size:16px;color:#ffffff;line-height: 28px;\">Добро пожаловать на наш уютный сайт TimMovie. Здесь тебя ждут лучшие фильмы и братская компания!</span>\n" +
            "                                            <br>\n" +
            "                                            <br>\n" +
            $"                                            <a style=\"font-family:Arial,sans-serif;font-size:14px;line-height:0;color:#938DB7;text-decoration: underline;cursor: pointer;display: flex;justify-content: center\" href={linkToConfirm}>Нажми на меня чтобы подтвердить регистрацию</a>";
        return GenerateMessageTemplate(header, content, "https://media2.giphy.com/media/4QxQgWZHbeYwM/200.gif",
            "Подтверждение регистрации");
    }

    public MessageMail GenerateResetPasswordMessage(string userName, string linkToConfirm)
    {
        return new MessageMail(
            $"<p>че) {userName} пароль забыл падла?</p> <br> <p>на, уебок {linkToConfirm}</p>", "Сброс пароля");
    }
    
    public MessageMail GenerateMessageTemplate(string header,string content,string picture ,string subject)
    {
        var msg = MailGenerator.GenerateMail(header, content, picture);
        return new MessageMail(msg, subject);
    }
}