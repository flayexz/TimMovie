using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.DTO.Messages;
using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Web.Controllers.Navbar;

public class TechnicalSupportController: Controller
{
    private readonly IRepository<Message> _messageRepository;
    private readonly IMapper _mapper;

    public TechnicalSupportController(IRepository<Message> messageRepository, IMapper mapper)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    //yes
    [HttpGet]
    public IActionResult Support()
    {
        return View("~/Views/Navbar/TechnicalSupport/Support.cshtml");
    }

    //yes
    public IActionResult MessageForSupportPage(MessageDto message)
    {
        return View("~/Views/Navbar/TechnicalSupport/Message.cshtml", message);
    }
    
    //Yes
    public IActionResult MessageForUserChat(MessageDto message)
    {
        return View("~/Views/Support/ModalWindowChat/Message.cshtml", message);
    }
    
    //Yes
    [HttpGet]
    public IActionResult NotificationAboutUserDisconnect()
    {
        return View("~/Views/Navbar/TechnicalSupport/NotificationInChat.cshtml", "Пользователь вышел из чата");
    }
    
    public IActionResult NotificationForUserChat(string content)
    {
        return View("~/Views/Support/ModalWindowChat/Notification.cshtml", content);
    }

    //yes
    [HttpGet]
    public IActionResult GetAllUserMessages()
    {
        var groupName = User.Identity?.Name;

        var messages = _messageRepository.Query
            .Where(message => message.GroupName == groupName)
            .OrderBy(message => message.Date)
            .ToList();
        var messagesDto = _mapper.Map<IEnumerable<MessageDto>>(messages);
        
        return View("~/Views/Support/ModalWindowChat/Messages.cshtml", messagesDto);
    }

    //yes
    [HttpGet]
    public IActionResult GetAllGroupMessagesForSupportPage(string groupName)
    {
        var messages = _messageRepository.Query
            .Where(message => message.GroupName == groupName)
            .OrderBy(message => message.Date)
            .ToList();
        var messagesDto = _mapper.Map<IEnumerable<MessageDto>>(messages);
        
        return View("~/Views/Navbar/TechnicalSupport/Messages.cshtml", messagesDto);
    }
}