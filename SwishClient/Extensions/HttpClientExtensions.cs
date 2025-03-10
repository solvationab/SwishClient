using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SwishClient.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            var content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");

            return client.PostAsync(requestUri, content);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            var content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");

            return client.PutAsync(requestUri, content);
        }

        public static async Task<T> GetFromJsonAsync<T>(this HttpClient client, string requestUri)
        {
            var response = await client.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<T>(responseStream);
            }
        }

        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            var content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");

            var httpRequestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = content
            };

            return client.SendAsync(httpRequestMessage);
        }
    }
}
