using SwishClient.Dto.Payments;
using SwishClient.Extensions;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SwishClient.Clients
{
    public class PaymentClient : IPaymentClient
    {
        private readonly HttpClient client;

        public PaymentClient(HttpClient client)
        {
            if (client == null) 
                throw new ArgumentNullException(nameof(client));

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
            var requestUri = $"/api/v2/paymentrequests/{instructionUuid}";

            var response = await client.PutAsJsonAsync(requestUri, request);

            var location = response.Headers.Location;

            var paymentRequestToken = response.Headers.GetValues("PaymentRequestToken").Single();

            return new CreateMcommercePaymentResponse(location, paymentRequestToken);
        }

        public Task<PaymentDto> GetPayment(string id)
        {
            var requestUri = $"/api/v1/paymentrequests/{id}";

            return client.GetFromJsonAsync<PaymentDto>(requestUri);
        }

        public async Task<PaymentDto> CancelPayment(string id)
        {
            var requestUri = $"/api/v1/paymentrequests/{id}";

            var response = await client.PatchAsJsonAsync(
                requestUri,
                new [] { new { op = "replace", path = "/status", value = "cancelled" } }
                );

            var stream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<PaymentDto>(stream);
        }
    }
}
