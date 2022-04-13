using TimMovie.Core.Classes;

namespace TimMovie.Core.Interfaces;

public interface IUserMessageService
{
    public MessageMail GenerateConfirmMessage(string userName, string linkToConfirm);
    public MessageMail GenerateResetPasswordMessage(string userName, string linkToConfirm);
    public MessageMail GenerateMessageTemplate(string header,string content,string picture,string subject);

}