using RestASPNet.Model;

namespace RestASPNet.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User? FindByUsername(string username);
    }
}
