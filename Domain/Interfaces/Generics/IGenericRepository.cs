using System.Linq.Expressions;

namespace Domain.Interfaces.Generics;
public interface IGenericRepository<T> where T:class{
    Task Add(T entity);
    Task AddRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    void Update(T entity);
    IAsyncEnumerable<T> GetAllAsync();
    IAsyncEnumerable<T> GetAllAsync(Expression<Func<T, bool>> expression);
    Task<T?> FindFisrtAsync(Expression<Func<T, bool>> expression);
}
