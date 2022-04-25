using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.DTO.Payment;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.Web.ViewModels.Payment;

namespace TimMovie.Web.Controllers.Payment;

[Authorize]
public class SubscribePaymentController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly IRepository<Subscribe> subscribeRepository;
    private readonly IPaymentService paymentService;
    private readonly IMapper mapper;

    public SubscribePaymentController(UserManager<User> userManager, IRepository<Subscribe> subscribeRepository,
        IPaymentService paymentService, IMapper mapper)
    {
        this.userManager = userManager;
        this.subscribeRepository = subscribeRepository;
        this.paymentService = paymentService;
        this.mapper = mapper;
    }


    [HttpGet]
    public async Task<IActionResult> Payment(Guid subscribeId, string? returnUrl)
    {
        var subscribe = subscribeRepository.Query.FirstOrDefault(new EntityByIdSpec<Subscribe>(subscribeId));
        if (subscribe is null)
            return BadRequest();
        var user = await userManager.FindByNameAsync(User.Identity!.Name);
        var payment = new SubscribePaymentViewModel
        {
            User = user,
            Subscribe = subscribe
        };
        return View("~/Views/Payment/Payment.cshtml", new SubscribePaymentWithCardViewModel {SubscribePaymentViewModel = payment, CardViewModel = new CardViewModel()});
    }


    [HttpPost]
    public async Task<IActionResult> Payment(CardViewModel cardViewModel ,Guid subscribeId, string? returnUrl)
    {
        var subscribeFromDb =
            subscribeRepository.Query.FirstOrDefault(
                new EntityByIdSpec<Subscribe>(subscribeId));

        if (subscribeFromDb is null)
            return BadRequest();

        var user = await userManager.FindByNameAsync(User.Identity!.Name);

        var subscribePaymentViewModel = new SubscribePaymentViewModel
        {
            User = user,
            Subscribe = subscribeFromDb
        };

        if (!string.IsNullOrEmpty(returnUrl))
            subscribePaymentViewModel.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
            return View("~/Views/Payment/Payment.cshtml",
                new SubscribePaymentWithCardViewModel
                    { SubscribePaymentViewModel = subscribePaymentViewModel, CardViewModel = cardViewModel });


        var subscribeDto = mapper.Map<SubscribePaymentDto>(subscribePaymentViewModel);
        var cardDto = mapper.Map<CardDto>(cardViewModel);
        var paymentResult = await paymentService.PaySubscribeAsync(subscribeDto, cardDto);

        if (paymentResult.IsFailure)
        {
            ModelState.AddModelError(string.Empty, paymentResult.Error);
            return View("~/Views/Payment/Payment.cshtml",
                new SubscribePaymentWithCardViewModel
                    { SubscribePaymentViewModel = subscribePaymentViewModel, CardViewModel = cardViewModel });
        }
        

        if (!string.IsNullOrEmpty(subscribePaymentViewModel.ReturnUrl))
            return LocalRedirect(subscribePaymentViewModel.ReturnUrl);

        return RedirectToAction("MainPage", "MainPage");
    }
}