using Domain.Models;

namespace Domain.Services
{
    public interface ITokenService
    {
        string GenerateActivationToken(Guid userId, string userEmail, DateTime expiry);
        bool ValidateActivationToken(string token);
        string GetUserIdFromToken(string token);
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
