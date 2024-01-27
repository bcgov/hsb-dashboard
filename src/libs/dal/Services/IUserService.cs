using HSB.Entities;

namespace HSB.DAL.Services;

public interface IUserService : IBaseService<User>
{
    IEnumerable<User> Find(Models.Filters.UserFilter filter);
    User? FindForId(int id, bool includePermissions);
    User? FindByKey(string key, bool includePermissions);
    User? FindByUsername(string username, bool includePermissions);
    IEnumerable<User> FindByEmail(string email, bool includePermissions);
}
