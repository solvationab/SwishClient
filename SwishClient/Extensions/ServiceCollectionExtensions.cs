using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwishClient.Clients;
using SwishClient.DelegatingHandlers;
using SwishClient.JsonConverters;
using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace SwishClient.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// This extension method adds Swish api to the service collection.
        ///
        /// To work it need a SwishConfig added to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The IServiceCollection to allow chaining</returns>
        public static IServiceCollection AddSwishClient(this IServiceCollection services)
        {
            // Add delegating handlers used by the Swish Client
            services
                .AddScoped<HttpLoggingHandler>();

            // Add the Swish API
            services.AddHttpClient<IPaymentClient, PaymentClient>()
                //.ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri("https://cpc.getswish.net/swish-cpcapi")) // Prod
                .ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri("https://mss.cpc.getswish.net")) // MSS
                .ConfigurePrimaryHttpMessageHandler(sp =>
                {
                    var clientCertificate = new X509Certificate2("", "swish");

                    var handler = new HttpClientHandler();

                    handler.SslProtocols = SslProtocols.Tls12;
                    handler.ClientCertificates.Add(clientCertificate);

                    // TODO: Make sure we verify the server certificate
                    // Issue URL: https://github.com/solvationab/SwishClient/issues/1
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                    return handler;
                })
                .AddHttpMessageHandler<HttpLoggingHandler>();

            return services;
        }

        public static IServiceCollection AddConfig<T>(this IServiceCollection services, IConfiguration configuration, string key)
            where T : class
        {
            var config = configuration
                    .GetRequiredSection(key)
                    .Get<T>()
                ?? throw new InvalidOperationException("Unable to get " + typeof(T).Name +" from section " + key +" in config");

            services.AddSingleton(config);

            return services;
        }

        public static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                //NumberHandling = JsonNumberHandling.AllowReadingFromString,
                //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverterEx() }
            };
        }
    }
}