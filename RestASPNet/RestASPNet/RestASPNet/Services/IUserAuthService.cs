using RestASPNet.Data.DTO.V1;
using RestASPNet.Model;

namespace RestASPNet.Services
{
    public interface IUserAuthService
    {
        User? FindByUsername(string username);
        User Create(AccountCredentialDTO accountCredentialDTO);
        User Update(User user);

        bool RevokeToken(string username);
    }
}
