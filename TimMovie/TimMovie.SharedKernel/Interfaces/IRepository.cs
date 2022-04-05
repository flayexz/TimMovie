namespace TimMovie.SharedKernel.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    void Create(TEntity item);
    IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
    void Update(TEntity item);
    void Delete(TEntity item);
    void Save();
}