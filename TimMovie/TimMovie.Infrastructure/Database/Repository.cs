using Microsoft.EntityFrameworkCore;
using TimMovie.SharedKernel.BaseEntities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Infrastructure.Database;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> CreateAsync(TEntity item)
    {
        await _dbSet.AddAsync(item);
        await SaveAsync();
        return await FindAsync(item);
    }

    // public async Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> predicate) =>
    //     _dbSet.AsNoTracking().Where(predicate);
    //
    // public async Task<TEntity?> GetFirstAsync(Func<TEntity, bool> predicate) => 
    //     _dbSet.AsNoTracking().Where(predicate).FirstOrDefault();

    public async Task<TEntity?> FindAsync(TEntity item) =>
        await _dbSet.AsNoTracking().FirstOrDefaultAsync(i => i.Equals(item));
    
    public async Task<TEntity?> FindByIdAsync(Guid id) =>
        await _dbSet.AsNoTracking().FirstOrDefaultAsync(i => i.Id.Equals(id));

    public async Task UpdateAsync(TEntity item)
    {
        _context.Entry(item).State = EntityState.Modified;
        await SaveAsync();
    }

    public async Task DeleteAsync(TEntity item)
    {
        _dbSet.Remove(item);
        await SaveAsync();
    }

    public Task SaveAsync() => _context.SaveChangesAsync();
}