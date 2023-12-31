using HSB.Entities;

namespace HSB.DAL.Services;

public interface IUserService : IBaseService<User>
{
    IEnumerable<User> Find(Models.Filters.UserFilter filter);
    User? FindByKey(string key);
    User? FindByUsername(string username);
    IEnumerable<User> FindByEmail(string email);
}
