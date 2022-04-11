using Microsoft.EntityFrameworkCore;
using TimMovie.SharedKernel.BaseEntities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Infrastructure.Database.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApplicationContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(ApplicationContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public IQueryable<TEntity> Query => _dbSet.AsQueryable();

    public async Task<TEntity?> AddAsync(TEntity item)
    {
        var entityEntry = await _dbSet.AddAsync(item);
        await _context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdateAsync(TEntity item)
    {
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity item)
    {
        _dbSet.Remove(item);
        await _context.SaveChangesAsync();
    }
}