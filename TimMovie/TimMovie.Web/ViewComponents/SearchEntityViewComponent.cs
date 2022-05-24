using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Interfaces;
using TimMovie.Web.ViewModels.SearchFromLayout;

namespace TimMovie.Web.ViewComponents;

public class SearchEntityViewComponent : ViewComponent
{
    private readonly ISearchEntityService _searchEntityService;

    public SearchEntityViewComponent(ISearchEntityService searchEntityService)
    {
        _searchEntityService = searchEntityService;
    }

    
    public IViewComponentResult Invoke(string namePart)
    {
        var searchResult = _searchEntityService.GetSearchEntityResultByNamePart(namePart);

        var viewModel = new SearchEntityViewModel
        {
            Films = searchResult.Films.Select(f => $"{f.Title}@{f.Year}"),
            Genres = searchResult.Genres.Select(g => g.Name),
            Actors = searchResult.Actors.Select(a => a.Surname is null ? $"{a.Name}" : $"{a.Name} {a.Surname}"),
            Producers = searchResult.Producers.Select(p => p.Surname is null ? $"{p.Name}" : $"{p.Name} {p.Surname}")
        };
        return View("~/Views/Shared/Components/SearchEntity/Default.cshtml", viewModel);
    }
}