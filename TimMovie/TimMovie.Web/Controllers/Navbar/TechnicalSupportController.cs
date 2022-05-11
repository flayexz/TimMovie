using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Classes;
using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.Web.Extensions;

namespace TimMovie.Web.Controllers.Navbar;

public class TechnicalSupportController: Controller
{
    private readonly IRepository<Message> _messageRepository;

    public TechnicalSupportController(IRepository<Message> messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public IActionResult Support()
    {
        return View("~/Views/Navbar/TechnicalSupport/Support.cshtml");
    }
    
    [HttpPost]
    public IActionResult ParseMessagesFromUnauthorizedUser(MessageWithContent[] messages)
    {
        return View("~/Views/Navbar/TechnicalSupport/Messages.cshtml", messages);
    }
    
    public IActionResult UserMessagesForSupportPage(Guid userId)
    {
        var messages = _messageRepository.Query
            .Where(message => message.User.Id == userId)
            .OrderBy(message => message.Date)
            .ToList()
            .Select(message => new MessageWithContent
            {
                Content = message.Content,
                ToUser = message.ToUser
            });
        return View("~/Views/Navbar/TechnicalSupport/Messages.cshtml", messages);
    }
    
    public IActionResult MessageForSupportPage(MessageWithContent message)
    {
        return View("~/Views/Navbar/TechnicalSupport/Message.cshtml", message);
    }
    
    public IActionResult MessageForUserChat(MessageWithContent message)
    {
        return View("~/Views/Support/ModalWindowChat/Message.cshtml", message);
    }
    
    public IActionResult NotificationAboutUserDisconnect()
    {
        return View("~/Views/Navbar/TechnicalSupport/NotificationInChat.cshtml", "Пользователь вышел из чата");
    }

    public IActionResult GetAllUserMessages()
    {
        Guid? userId = User.Identity.IsAuthenticated ? User.GetUserId() : null;
        if (userId is null)
        {
            return View("~/Views/Support/ModalWindowChat/Messages.cshtml", Enumerable.Empty<MessageWithContent>());
        }
        var messages = _messageRepository.Query
            .Where(message => message.User.Id == userId.Value)
            .OrderBy(message => message.Date)
            .ToList()
            .Select(message => new MessageWithContent
            {
                Content = message.Content,
                ToUser = message.ToUser
            });
        
        return View("~/Views/Support/ModalWindowChat/Messages.cshtml", messages);
    }
}