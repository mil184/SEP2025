using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Helpers
{
    public static class SafeRefHelper
    {
        public static string SafeRef(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "null";

            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));

            return Convert.ToHexString(bytes, 0, 8);
        }
    }
}
