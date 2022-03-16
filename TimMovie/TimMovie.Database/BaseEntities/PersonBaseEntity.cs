namespace TimMovie.Database.BaseEntities;

public abstract class PersonBaseEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Photo { get; set; }
}