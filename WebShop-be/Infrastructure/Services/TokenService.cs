using Domain.Models;
using Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly byte[] _hmacKey;

        public TokenService(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"]!;
            _issuer = configuration["Jwt:Issuer"]!;
            _audience = configuration["Jwt:Audience"]!;
            _hmacKey = Encoding.UTF8.GetBytes(configuration["Jwt:HmacSecret"]!);
        }

        public string GenerateActivationToken(Guid userId, string userEmail, DateTime expiry)
        {
            var payload = $"{userId}|{userEmail}|{expiry:o}";
            var hmac = new HMACSHA256(_hmacKey);
            var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(payload)));

            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{payload}|{hash}"));

            return token;
        }

        public bool ValidateActivationToken(string token)
        {
            string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(token));

            var parts = decoded.Split('|');
            if (parts.Length != 4)
                throw new Exception("Invalid token.");

            var userId = parts[0];
            var email = parts[1];
            var expiryString = parts[2];
            var sentHash = parts[3];

            if (!DateTime.TryParse(expiryString, out var expiry))
                throw new Exception("Invalid expiry.");

            if (DateTime.UtcNow > expiry)
                throw new Exception("Link expired.");

            var payload = $"{userId}|{email}|{expiryString}";
            var hmac = new HMACSHA256(_hmacKey);
            var expectedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(payload)));

            if (sentHash != expectedHash)
                throw new Exception("Invalid token integrity.");

            return true;
        }

        public string GetUserIdFromToken(string token)
        {
            string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(token));

            var parts = decoded.Split('|');
            if (parts.Length != 4)
                throw new Exception("Invalid token.");

            return parts[0];
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
