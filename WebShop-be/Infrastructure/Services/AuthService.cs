using Domain.Dtos;
using Domain.Models;
using Domain.Services;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;


        public AuthService(IUserService userService)
        {
            _userService = userService;
        }

        public User? RegisterUser(RegisterDto userDto)
        {
            var user = new User() { Name = userDto.Name, Surname = userDto.Surname, Email = userDto.Email, Password = "" };
            user.Password = HashPassword(userDto.Password);
            return _userService.Create(user);
        }

        public User? LoginUser(LoginDto loginDto)
        {
            var user = _userService.GetByEmail(loginDto.Email);
            return CheckPassword(loginDto.Password, user) ? user : null;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 13);
        }

        private bool CheckPassword(string password, User user)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }
    }
}
