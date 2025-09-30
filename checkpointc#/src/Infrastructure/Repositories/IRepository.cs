using System.Linq;

namespace Infrastructure.Repositories;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    IQueryable<T> Query();
}
