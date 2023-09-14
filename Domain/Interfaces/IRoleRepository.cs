using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Generics.GenericById;

namespace Domain.Interfaces;
public interface IRoleRepository: IGenericRepositoryIntId<Role>{
    Task<Role?> GetRolByRoleName(Roles rol);
    Task<Role?> GetRolByRoleName(string rol);
}
