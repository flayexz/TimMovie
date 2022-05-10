using TimMovie.Core.Entities;

namespace TimMovie.Web.ViewModels.SearchFromLayout;

public class SearchEntityViewModel
{
    public IEnumerable<string>? Films { get; set; }
    
    public IEnumerable<string>? Genres { get; set; }
    
    public IEnumerable<string>? Actors { get; set; }
    
    public IEnumerable<string>? Producers { get; set; }
}