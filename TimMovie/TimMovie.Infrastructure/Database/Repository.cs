using Microsoft.EntityFrameworkCore;
using TimMovie.SharedKernel.BaseEntities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Infrastructure.Database;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly ApplicationContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(ApplicationContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public IQueryable<TEntity> Query => _dbSet.AsQueryable();

    public async Task<List<TEntity>> GetAllAsync() =>
        await _dbSet.ToListAsync();
    
    public async Task<TEntity?> AddAsync(TEntity item)
    {
        var entityEntry = await _dbSet.AddAsync(item);
        return entityEntry.Entity;
    }

    public void Update(TEntity item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }

    public void Delete(TEntity item)
    {
        _dbSet.Remove(item);
    }
    
    public async Task SaveChangesAsync()=>
        await _context.SaveChangesAsync();
}