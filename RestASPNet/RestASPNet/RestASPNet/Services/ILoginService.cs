using RestASPNet.Data.DTO.V1;

namespace RestASPNet.Services
{
    public interface ILoginService
    {
        TokenDTO? ValidateCredentials(UserDTO userDTO);
        TokenDTO? ValidateCredentials(TokenDTO token);
        bool RevokeToken(string username);
        AccountCredentialDTO Create(AccountCredentialDTO userDTO);
    }
}
