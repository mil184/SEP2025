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

        public async Task<PaymentFinalizationResponseDto> FinalizeAsync(PaymentFinalizationRequestDto dto, CancellationToken ct = default)
        {
            // sends request to PSP
            using var response = await _http.PostAsJsonAsync("api/bank-payment-requests/finalize", dto, ct);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PaymentFinalizationResponseDto>(cancellationToken: ct);
            return result ?? throw new InvalidOperationException("PSP response is empty.");
        }
    }
}
