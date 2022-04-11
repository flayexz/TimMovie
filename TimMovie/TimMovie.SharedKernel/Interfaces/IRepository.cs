using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.SharedKernel.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    IQueryable<TEntity> Query { get; }
    Task<TEntity?> AddAsync(TEntity item);
    Task UpdateAsync(TEntity item);
    Task DeleteAsync(TEntity item);
}