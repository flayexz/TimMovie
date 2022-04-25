using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.SharedKernel.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> Query { get; }
    Task<TEntity?> AddAsync(TEntity item);
    Task<List<TEntity>> GetAllAsync();
    void Update(TEntity item);
    void Delete(TEntity item);
}