using Microsoft.AspNetCore.Http;

namespace Domain.Dtos
{
    public class QrValidateRequest
    {
        public IFormFile File { get; set; } = default!;
    }
}
