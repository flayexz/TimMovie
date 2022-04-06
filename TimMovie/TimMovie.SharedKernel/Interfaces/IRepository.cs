using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.SharedKernel.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> AddAsync(TEntity item);
    // IEnumerable<TEntity> GetAsync(Func<TEntity, bool> predicate);
    // TEntity? GetFirstAsync(Func<TEntity, bool> predicate);
    Task<TEntity?> FindAsync(TEntity item);
    Task UpdateAsync(TEntity item);
    Task DeleteAsync(TEntity item);
    Task SaveAsync();
}