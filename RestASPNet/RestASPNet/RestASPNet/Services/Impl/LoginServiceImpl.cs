using Microsoft.IdentityModel.JsonWebTokens;
using RestASPNet.Auth.Config;
using RestASPNet.Auth.Contract;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Model;
using RestASPNet.Repositories;
using System.Security.Claims;

namespace RestASPNet.Services.Impl
{
    public class LoginServiceImpl : ILoginService
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private readonly IUserAuthService _userauthService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenService;
        private readonly TokenConfiguration _configurations;

        public LoginServiceImpl(IUserAuthService userauthService, IPasswordHasher passwordHasher, ITokenGenerator tokenService, TokenConfiguration configurations)
        {
            _userauthService = userauthService;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _configurations = configurations;
        }
        public TokenDTO? ValidateCredentials(UserDTO userDTO)
        {
            var user = _userauthService.FindByUsername(userDTO.UserName);
            if (user == null) { return null; };
            if(!_passwordHasher.Verify(userDTO.Password, user.Password)){ return null; };

            return GenerateToken(user);

        }

        public TokenDTO? ValidateCredentials(TokenDTO token)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token.AccessToken);
            var username = principal.Identity?.Name;
            if (username == null) { return null; }
            var user = _userauthService.FindByUsername(username);
            if (user == null) { return null; }
            if (user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now) { return null; }
            return GenerateToken(user, principal.Claims);
        }
        public AccountCredentialDTO Create(AccountCredentialDTO userDto)
        {
            var user = _userauthService.Create(userDto);

            return new AccountCredentialDTO
            {
                UserName = user.Username,
                FullName = user.FullName,
                Password = "*******"
            };

        }

        private TokenDTO GenerateToken(User user, IEnumerable<Claim>? existingClaims = null)
        {
            var claims = existingClaims?.ToList() ?? new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);

            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configurations.DaysToExpire);

            _userauthService.Update(user);

            var createdDate = DateTime.UtcNow.ToString(DATE_FORMAT);
            var expirationDate = DateTime.UtcNow.AddMinutes(_configurations.Minutes).ToString(DATE_FORMAT);

            return new TokenDTO(true, createdDate,expirationDate,accessToken,refreshToken);

        }
        public bool RevokeToken(string username)
        {
            return _userauthService.RevokeToken(username);
        }

    }
}
