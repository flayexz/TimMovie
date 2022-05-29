using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Services.Person;
using TimMovie.Web.ViewModels.Person;

namespace TimMovie.Web.Controllers.Person;

public class PersonController: Controller
{
    private readonly PersonService _personService;
    private readonly IMapper _mapper;

    public PersonController(PersonService personService, IMapper mapper)
    {
        _personService = personService;
        _mapper = mapper;
    }

    [HttpGet("[controller]/{id:guid}")]
    public IActionResult GetPersonPage(Guid id)
    {
        var person = _personService.GetPersonById(id);
        var personView = _mapper.Map<PersonViewModel>(person);
        return View("~/Views/Person/Person.cshtml", personView);
    }
}