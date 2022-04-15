using System.Linq.Expressions;
using TimMovie.SharedKernel.BaseEntities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Query;

public class QueryExecutor<TEntity>
    where TEntity: BaseEntity
{
    private IQueryable<TEntity> _query;
    private readonly IRepository<TEntity> _repository;

    public QueryExecutor(IQueryable<TEntity> query, IRepository<TEntity> repository)
    {
        _query = query;
        _repository = repository;
    }

    public IEnumerable<TEntity> GetEntitiesWithPagination(int amountSkip, int amountTake)
    {
        return _query
            .Skip(amountSkip)
            .Take(amountTake)
            .ToList();
    }

    public QueryExecutor<TEntity> IncludeInResult<TProperty>(
        Expression<Func<TEntity, TProperty>> navigationPathToProperty)
    {
        _query = _repository.Include(_query, navigationPathToProperty);
        
        return this;
    }
}