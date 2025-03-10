using System;
using System.Net.Http;
using System.Threading.Tasks;
using SwishClient.Dto.QrCodes;
using SwishClient.Extensions;

namespace SwishClient.Clients
{
    public class SwishQrCodeClient : ISwishQrCodeClient, IDisposable
    {
        private readonly HttpClient client;

        public SwishQrCodeClient(HttpClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            this.client = client;
        }

        public async Task<byte[]> GetPaymentQrCode(GetPaymentQrCodeRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var requestUri = $"api/v1/commerce";

            var response = client.PostAsJsonAsync(requestUri, request);

            return await response.Result.Content.ReadAsByteArrayAsync();
        }

        public async Task<byte[]> GetPrefilledQrCode(GetPrefilledQrCodeRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var requestUri = $"api/v1/prefilled";

            var response = client.PostAsJsonAsync(requestUri, request);

            return await response.Result.Content.ReadAsByteArrayAsync();
        }
        public void Dispose()
        {
            client.Dispose();
        }
    }
}