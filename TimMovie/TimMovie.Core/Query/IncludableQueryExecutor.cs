using System.Linq.Expressions;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Query;

public class IncludableQueryExecutor<TEntity, TIncludableProperty> : QueryExecutor<TEntity>
    where TEntity : class
{
    public IncludableQueryExecutor(IQueryable<TEntity> query, IRepository<TEntity> repository) 
        : base(query, repository)
    {
    }
    
    public QueryExecutor<TEntity> ThenIncludeInResult<TProperty>(
        Expression<Func<TIncludableProperty,TProperty>> navigationPathToProperty)
    {
        Query = Repository.ThenInclude(Query, navigationPathToProperty);
        
        return new IncludableQueryExecutor<TEntity,TProperty>(Query, Repository);
    }
}