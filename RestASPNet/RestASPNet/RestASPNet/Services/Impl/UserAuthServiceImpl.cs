using Microsoft.Identity.Client;
using RestASPNet.Auth.Contract;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Model;
using RestASPNet.Repositories;

namespace RestASPNet.Services.Impl
{
    public class UserAuthServiceImpl : IUserAuthService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public UserAuthServiceImpl(IUserRepository repository, IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }


        public User? FindByUsername(string username)
        {
            return _repository.FindByUsername(username);
        }
        public User Update(User user)
        {
            return _repository.Update(user);
        }
        public User Create(AccountCredentialDTO accountCredentialDTO)
        {
            if (accountCredentialDTO == null) { throw new ArgumentNullException(nameof(accountCredentialDTO)); }
            var user = new User
            {
                Username = accountCredentialDTO.UserName,
                FullName = accountCredentialDTO.FullName,
                Password = _passwordHasher.Hash(accountCredentialDTO.Password),
                RefreshToken = string.Empty,
                RefreshTokenExpiryTime = null

            };

            return _repository.Create(user);
        }
        public bool RevokeToken(string username)
        {
            var user = _repository.FindByUsername(username);
            if (user == null) { return false; }
            user.RefreshToken = string.Empty;
            user.RefreshTokenExpiryTime = null;
            _repository.Update(user);
            return true;
        }

    }
}
