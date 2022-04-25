using System.Linq.Expressions;
using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.SharedKernel.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    IQueryable<TEntity> Query { get; }
    Task<TEntity?> AddAsync(TEntity item);
    Task<List<TEntity>> GetAllAsync();
    Task SaveChangesAsync();
    void Update(TEntity item);
    void Delete(TEntity item);

    IQueryable<TEntity> Include<TProperty>(
        IQueryable<TEntity> query,
        Expression<Func<TEntity, TProperty>> navigationPathToProperty);
}