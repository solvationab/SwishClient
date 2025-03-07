using SwishClient.Dto.Payments;
using SwishClient.Extensions;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SwishClient.Clients
{
    public class PaymentClient : IPaymentClient, IDisposable
    {
        private readonly HttpClient client;

        public PaymentClient(HttpClient client)
        {
            if (client == null) 
                throw new ArgumentNullException(nameof(client));

            this.client = client;
        }

        /// <summary>
        /// Create a new E-commerce payment
        /// </summary>
        /// <param name="instructionUuid">The identifier of the payment request to be saved. Example: 11A86BE70EA346E4B1C39C874173F088 UUID Regex, [0-9A-F]{32}</param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CreateEcommercePaymentResponse> CreateEcommercePayment(string instructionUuid, CreateEcommercePaymentRequest request)
        {
            if (Regex.IsMatch(instructionUuid, "^[0-9A-F]{32}$") == false)
                throw new ArgumentException("UUID Regex, [0-9A-F]{32}", nameof(instructionUuid));

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var requestUri = $"api/v2/paymentrequests/{instructionUuid}";

            var response = await client.PutAsJsonAsync(requestUri, request);

            var location = response.Headers.Location;

            return new CreateEcommercePaymentResponse(location);
        }

        public async Task<CreateMcommercePaymentResponse> CreateMcommercePayment(string instructionUuid, CreateMcommercePaymentRequest request)
        {
            if (Regex.IsMatch(instructionUuid, "^[0-9A-F]{32}$") == false)
                throw new ArgumentException("UUID Regex, [0-9A-F]{32}", nameof(instructionUuid));

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var requestUri = $"api/v2/paymentrequests/{instructionUuid}";

            var response = await client.PutAsJsonAsync(requestUri, request);

            var location = response.Headers.Location;

            var paymentRequestToken = response.Headers.GetValues("PaymentRequestToken").Single();

            return new CreateMcommercePaymentResponse(location, paymentRequestToken);
        }

        public Task<PaymentDto> GetPayment(string id)
        {
            if (Regex.IsMatch(id, "^[0-9A-F]{32}$") == false)
                throw new ArgumentException("UUID Regex, [0-9A-F]{32}", nameof(id));

            var requestUri = $"api/v1/paymentrequests/{id}";

            return client.GetFromJsonAsync<PaymentDto>(requestUri);
        }

        public async Task<PaymentDto> CancelPayment(string id)
        {
            if (Regex.IsMatch(id, "^[0-9A-F]{32}$") == false)
                throw new ArgumentException("UUID Regex, [0-9A-F]{32}", nameof(id));

            var requestUri = $"api/v1/paymentrequests/{id}";

            var response = await client.PatchAsJsonAsync(
                requestUri,
                new [] { new { op = "replace", path = "/status", value = "cancelled" } }
                );

            var stream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<PaymentDto>(stream);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
