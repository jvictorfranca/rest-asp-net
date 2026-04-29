using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Services;
using System.Runtime.CompilerServices;

namespace RestASPNet.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private ILoginService _loginService;
        private readonly IUserAuthService _userAuthService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILoginService loginService, IUserAuthService userAuthService, ILogger<AuthController> logger)
        {
            _loginService = loginService;
            _userAuthService = userAuthService;
            _logger = logger;
        }

        [HttpPost("signin", Name = "SignInUser")]
        [AllowAnonymous]
        public IActionResult SignIn([FromBody] UserDTO user)
        {
            _logger.LogInformation("SignIn attempt for user: {UserName}", user.UserName);
            if (user == null) { return BadRequest("Invalid client request"); }
            var existingUser = _userAuthService.FindByUsername(user.UserName);
            if (existingUser == null) { return Unauthorized(); }
            var token = _loginService.ValidateCredentials(user);
            if (token == null) { return Unauthorized(); }
            return Ok(token);

        }

        [HttpPost("refresh", Name = "RefreshToken")]
        public IActionResult Refresh([FromBody] TokenDTO tokenDto)
        {
            if (tokenDto == null) { return BadRequest("Invalid client request"); }
            var newToken = _loginService.ValidateCredentials(tokenDto);
            if (newToken == null) { return Unauthorized(); }
            return Ok(newToken);
        }

        [HttpPost("revoke", Name = "RevokeToken")]
        [Authorize]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;
            if (string.IsNullOrEmpty(username)) { return BadRequest("Invalid client request"); }
            var result = _loginService.RevokeToken(username);
            if(!result) { return BadRequest("Failed to revoke token"); }
            return NoContent();
        }

        [HttpPost("create", Name = "CreateUser")]
        [AllowAnonymous]
        public IActionResult Create([FromBody] AccountCredentialDTO user)
        {
            if (user == null) { return BadRequest("Invalid client request"); }
            var result = _loginService.Create(user);
            return Ok(result);
        }
    }
}
