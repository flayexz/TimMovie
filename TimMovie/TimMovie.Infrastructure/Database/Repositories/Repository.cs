using Microsoft.EntityFrameworkCore;
using TimMovie.SharedKernel.BaseEntities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Infrastructure.Database.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApplicationContext Context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(ApplicationContext context)
    {
        Context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> AddAsync(TEntity item)
    {
        var entityEntry = await _dbSet.AddAsync(item);
        await SaveAsync();
        return entityEntry.Entity;
    }

    public async Task<TEntity?> FindAsync(TEntity item) =>
        await _dbSet.FirstOrDefaultAsync(i => i.Equals(item));
    
    public async Task<TEntity?> FindByIdAsync(Guid id) =>
        await _dbSet.FirstOrDefaultAsync(i => i.Id.Equals(id));

    public async Task<List<TEntity>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task UpdateAsync(TEntity item)
    {
        Context.Entry(item).State = EntityState.Modified;
        await SaveAsync();
    }

    public async Task DeleteAsync(TEntity item)
    {
        _dbSet.Remove(item);
        await SaveAsync();
    }

    public Task SaveAsync() => Context.SaveChangesAsync();
}