using AutoMapper;
using Domain.Dtos;
using Domain.Models;
using Domain.Services;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Validators;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        private readonly IMapper _mapper;

        private readonly LoginValidator _loginValidator;
        private readonly RegisterValidator _registerValidator;
        private readonly PasswordValidator _passwordValidator;

        public AuthController(IAuthService authService, IUserService userService, ITokenService tokenService, IMapper mapper, LoginValidator loginValidator, PasswordValidator passwordValidator, RegisterValidator registerValidator)
        {
            _authService = authService;
            _userService = userService;
            _tokenService = tokenService;

            _mapper = mapper;

            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _passwordValidator = passwordValidator;
        }

        [HttpGet("current-user")]
        public ActionResult<UserDto> GetCurrentUser()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var user = _userService.GetById(GuidHelper.GetGuidFromString(userIdClaim));
            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<UserDto>(user);
        }

        [HttpPost("login")]
        public ActionResult Login(LoginDto loginDto)
        {
            var result = _authService.LoginUser(loginDto);
            if (result == null)
            {
                return BadRequest("Invalid user.");
            }

            var validator = _loginValidator.Validate(loginDto);
            if (!validator.IsValid)
            {
                return BadRequest(validator.Errors);
            }

            User user = _userService.GetByEmail(loginDto.Email);
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            return Ok(new { accessToken, refreshToken });
        }

        [HttpPost("register")]
        public ActionResult<User> Register(RegisterDto userDto)
        {
            var user = _authService.RegisterUser(userDto);

            var registerValidator = _registerValidator.Validate(userDto);
            if (!registerValidator.IsValid)
            {
                return BadRequest(registerValidator.Errors);
            }

            var passwordValidation = _passwordValidator.Validate(userDto);
            if (!passwordValidation.IsValid)
            {
                return BadRequest(passwordValidation.Errors);
            }

            return Ok(user);
        }
    }
}
