using Domain.Entities.Generics;

namespace Domain.Entities;
public class Role: BaseEntityWithIntId{
    public string Description { get; set; } = String.Empty;

    public IEnumerable<User> Users { get; set; } = new HashSet<User>();
}