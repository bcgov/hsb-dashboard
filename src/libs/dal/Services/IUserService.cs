using HSB.Entities;

namespace HSB.DAL.Services;

public interface IUserService : IBaseService<User>
{
    IEnumerable<User> FindAll();

    User? FindByKey(Guid key);
    User? FindByUsername(string username);
    IEnumerable<User> FindByEmail(string email);
}
