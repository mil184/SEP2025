using Domain.Dtos;
using Domain.Models;

namespace Domain.Services
{
    public interface IAuthService
    {
        public User? RegisterUser(RegisterDto userDto);
        public User? LoginUser(LoginDto loginDto);
    }
}
