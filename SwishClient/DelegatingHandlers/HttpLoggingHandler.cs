using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SwishClient.DelegatingHandlers
{
    public class HttpLoggingHandler : DelegatingHandler
    {
        private readonly ILogger<HttpLoggingHandler> logger;

        public HttpLoggingHandler(ILogger<HttpLoggingHandler> logger)
        {
            this.logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var req = request;
            var id = Guid.NewGuid().ToString();
            var msg = $"[{id} -   Request]";

            logger.LogDebug($"{msg}========Start==========");
            logger.LogDebug($"{msg} {req.Method} {req.RequestUri.PathAndQuery} {req.RequestUri.Scheme}/{req.Version}");
            logger.LogDebug($"{msg} Host: {req.RequestUri.Scheme}://{req.RequestUri.Host}");

            foreach (var header in req.Headers)
                logger.LogDebug($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

            if (req.Content != null)
            {
                foreach (var header in req.Content.Headers)
                    logger.LogDebug($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

                if (req.Content is StringContent || IsTextBasedContentType(req.Headers) || IsTextBasedContentType(req.Content.Headers))
                {
                    var result = await req.Content.ReadAsStringAsync();

                    logger.LogDebug($"{msg} Content:");
                    logger.LogDebug($"{msg} {result}");

                }
            }

            var start = DateTime.Now;

            var response = await base.SendAsync(request, cancellationToken);

            var end = DateTime.Now;

            logger.LogDebug($"{msg} Duration: {end - start}");
            logger.LogDebug($"{msg}==========End==========");

            msg = $"[{id} - Response]";
            logger.LogDebug($"{msg}=========Start=========");

            var resp = response;

            logger.LogDebug($"{msg} {req.RequestUri.Scheme.ToUpper()}/{resp.Version} {(int)resp.StatusCode} {resp.ReasonPhrase}");

            foreach (var header in resp.Headers)
                logger.LogDebug($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

            if (resp.Content != null)
            {
                foreach (var header in resp.Content.Headers)
                    logger.LogDebug($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

                if (resp.Content is StringContent || IsTextBasedContentType(resp.Headers) || IsTextBasedContentType(resp.Content.Headers))
                {
                    start = DateTime.Now;
                    var result = await resp.Content.ReadAsStringAsync();
                    end = DateTime.Now;

                    logger.LogDebug($"{msg} Content:");
                    logger.LogDebug($"{msg} {result}");
                    logger.LogDebug($"{msg} Duration: {end - start}");
                }
            }

            logger.LogDebug($"{msg}==========End==========");

            return response;
        }

        readonly string[] types = { "html", "text", "xml", "json", "txt", "x-www-form-urlencoded" };

        bool IsTextBasedContentType(HttpHeaders headers)
        {
            if (!headers.TryGetValues("Content-Type", out var values))
                return false;

            var header = string.Join(" ", values).ToLowerInvariant();

            return types.Any(t => header.Contains(t));
        }
    }
}
