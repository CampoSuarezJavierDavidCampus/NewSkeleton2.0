using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories.Generics;
public abstract class GenericRepository<T> where T : class{
    protected readonly DbSet<T> _Entity;
    protected readonly DataContext _Context;

    public GenericRepository(DataContext context){
        _Context = context;
        _Entity = context.Set<T>();
    }
    public virtual async Task Add(T entity)=>await _Entity.AddAsync(entity);
    public virtual async Task AddRange(IEnumerable<T> entities)=>await _Entity.AddRangeAsync(entities);
    public virtual void Remove(T entity)=>_Entity.Remove(entity); 
    public virtual void RemoveRange(IEnumerable<T> entities)=>_Entity.RemoveRange(entities);
    public virtual void Update(T entity)=> _Entity.Update(entity);
    public virtual async Task<T?> FindFisrtAsync(Expression<Func<T, bool>> expression)=>await _Entity.Where(expression).FirstOrDefaultAsync();
    public virtual async IAsyncEnumerable<T> GetAllAsync(){
        await foreach (var entity in _Entity.AsAsyncEnumerable()){
            yield return entity;
        }
    }
    public virtual async IAsyncEnumerable<T> GetAllAsync(Expression<Func<T, bool>> expression){
        await foreach (var entity in _Entity.Where(expression).AsAsyncEnumerable()){
            yield return entity;
        }
    }
    

}