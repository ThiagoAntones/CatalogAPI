using System.Linq.Expressions;

namespace CatalogAPI.Repositories;

public interface IRepository<T>
{
    //tomar cuidado para não violar o principio ISP
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
}
