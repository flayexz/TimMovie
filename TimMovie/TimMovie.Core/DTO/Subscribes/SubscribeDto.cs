using TimMovie.Core.Entities;

namespace TimMovie.Core.DTO.Subscribes;

public class SubscribeDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }
    
    public List<Film> Films { get; set; }
    
    public List<Genre> Genres { get; set; }
}