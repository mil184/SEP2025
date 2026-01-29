using Domain.Dtos;
using System.Net.Http.Json;

namespace Infrastructure.Clients
{
    public class BankClient
    {
        private readonly HttpClient _http;

        public BankClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<BankPaymentResponseDto> GetRedirectAsync(BankPaymentRequestDto dto, CancellationToken ct = default)
        {
            // sends request to Bank
            using var response = await _http.PostAsJsonAsync("api/payments/bank-payment-request", dto, ct);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<BankPaymentResponseDto>(cancellationToken: ct);
            return result ?? throw new InvalidOperationException("Bank response is empty.");
        }
    }
}
