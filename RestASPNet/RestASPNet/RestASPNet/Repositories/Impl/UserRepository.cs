using RestASPNet.Model;
using RestASPNet.Model.Context;

namespace RestASPNet.Repositories.Impl
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(MSSQLContext context) : base(context)
        {}

        public User? FindByUsername(string username)
        {
            return _context.Users.SingleOrDefault(x => x.Username == username);
        }
    }
}
