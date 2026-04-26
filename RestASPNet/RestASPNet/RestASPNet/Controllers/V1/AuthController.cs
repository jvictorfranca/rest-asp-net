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
    }
}
