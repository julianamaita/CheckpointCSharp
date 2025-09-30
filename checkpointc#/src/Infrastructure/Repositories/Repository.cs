using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _ctx;
    private readonly DbSet<T> _dbSet;

    public Repository(DbContext ctx)
    {
        _ctx = ctx;
        _dbSet = _ctx.Set<T>();
    }

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public void Update(T entity) => _dbSet.Update(entity);
    public void Remove(T entity) => _dbSet.Remove(entity);
    public IQueryable<T> Query() => _dbSet.AsQueryable();
}
