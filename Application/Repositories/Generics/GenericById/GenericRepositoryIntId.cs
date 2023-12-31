using Domain.Entities.Generics;
using Persistence;

namespace Application.Repositories.Generics.GenericById;
public abstract class GenericRepositoryIntId<T> : GenericRepository<T> where T : BaseEntityWithIntId{
    public GenericRepositoryIntId(DataContext context) : base(context){}
    public virtual async Task<T?> GetByIdAsync(int id)=> await _Entity.FindAsync(id);
}
