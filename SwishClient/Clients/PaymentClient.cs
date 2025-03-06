using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using SwishClient.Dto.Payments;
using System.IO;

namespace SwishClient.Clients
{
    public class PaymentClient : IPaymentClient
    {
        private readonly HttpClient client;

        public PaymentClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<CreateEcommercePaymentResponse> CreateEcommercePayment(string instructionUuid, CreateEcommercePaymentRequest request)
        {
            var requestUri = $"/api/v2/paymentrequests/{instructionUuid}";

            var response = await client.PutAsJsonAsync(requestUri, request);

            var location = response.Headers.Location;

            return new CreateEcommercePaymentResponse(location);
        }

        public async Task<CreateMcommercePaymentResponse> CreateMcommercePayment(string instructionUuid, CreateMcommercePaymentRequest request)
        {
            var response = await client.PutAsJsonAsync($"/api/v2/paymentrequests/{instructionUuid}", request);

            var location = response.Headers.Location;

            var paymentRequestToken = response.Headers.GetValues("PaymentRequestToken").Single();

            return new CreateMcommercePaymentResponse(location, paymentRequestToken);
        }

        public async Task<PaymentDto> GetPayment(string id)
        {
            var response = await client.GetFromJsonAsync<PaymentDto>($"/api/v1/paymentrequests/{id}");

            return response;
        }

        public async Task<PaymentDto> CancelPayment(string id)
        {
            var response = await client.PatchAsJsonAsync(
                $"/api/v1/paymentrequests/{id}",
                new [] { new { op = "replace", path = "/status", value = "cancelled" } }
                );

            return await response.Content.ReadFromJsonAsync<PaymentDto>();
        }
    }
}
