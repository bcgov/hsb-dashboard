using HSB.Entities;

namespace HSB.DAL.Services;

public interface IGroupService : IBaseService<Group>
{
    Group? FindForId(int id);
}
