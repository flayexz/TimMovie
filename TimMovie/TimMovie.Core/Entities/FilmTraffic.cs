using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class FilmTraffic
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid FilmId { get; set; }
    
    public int Count { get; set; }
}