using Microsoft.EntityFrameworkCore;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Infrastructure.Database;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public void Create(TEntity item)
    {
        _dbSet.Add(item);
        Save();
    }

    public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate) =>
        _dbSet.AsNoTracking().Where(predicate);

    public TEntity? GetFirst(Func<TEntity, bool> predicate) => 
        _dbSet.AsNoTracking().Where(predicate).FirstOrDefault();

    public void Update(TEntity item)
    {
        _context.Entry(item).State = EntityState.Modified;
        Save();
    }

    public void Delete(TEntity item)
    {
        _dbSet.Remove(item);
        Save();
    }

    public void Save() => _context.SaveChanges();
}