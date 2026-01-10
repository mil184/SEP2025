using Domain.Dtos;
using System.Net.Http.Json;

namespace Infrastructure.Clients
{
    public class PspClient
    {
        private readonly HttpClient _http;

        public PspClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<PaymentInitializationResponseDto> InitializeAsync(PaymentInitializationRequestDto dto, CancellationToken ct = default)
        {
            // sends request to PSP
            using var response = await _http.PostAsJsonAsync("api/payments/initialize", dto, ct);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PaymentInitializationResponseDto>(cancellationToken: ct);
            return result ?? throw new InvalidOperationException("PSP response is empty.");
        }
    }
}
