using Domain.Dtos;
using Domain.Models;
using Domain.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserService userService, ILogger<AuthService> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public User? RegisterUser(RegisterDto userDto)
        {
            _logger.LogInformation("Register attempt for email {Email}", userDto.Email);

            try
            {
                var existing = _userService.GetByEmail(userDto.Email);
                if (existing != null)
                {
                    _logger.LogWarning("Register failed: email already exists {Email}", userDto.Email);
                    return null;
                }

                var user = new User
                {
                    Name = userDto.Name,
                    Surname = userDto.Surname,
                    Email = userDto.Email,
                    Password = ""
                };

                user.Password = HashPassword(userDto.Password);

                var created = _userService.Create(user);

                if (created == null)
                {
                    _logger.LogWarning("Register failed: user service returned null for email {Email}", userDto.Email);
                    return null;
                }

                _logger.LogInformation("Register succeeded for userId {UserId} email {Email}", created.Id, created.Email);
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register error for email {Email}", userDto.Email);
                return null;
            }
        }

        public User? LoginUser(LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for email {Email}", loginDto.Email);

            try
            {
                var user = _userService.GetByEmail(loginDto.Email);

                if (user == null)
                {
                    // _logger.LogWarning("Login failed: user not found for email {Email}", loginDto.Email);
                    return null;
                }

                if (!CheckPassword(loginDto.Password, user))
                {
                    _logger.LogWarning("Login failed: invalid credentials for email {Email} userId {UserId}", user.Email, user.Id);
                    return null;
                }

                _logger.LogInformation("Login succeeded for userId {UserId} email {Email}", user.Id, user.Email);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for email {Email}", loginDto.Email);
                return null;
            }
        }

        private string HashPassword(string password)
        {
            _logger.LogDebug("Hashing password");
            return BCrypt.Net.BCrypt.HashPassword(password, 13);
        }

        private bool CheckPassword(string password, User user)
        {
            _logger.LogDebug("Verifying password for userId {UserId}", user.Id);
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }
    }
}
