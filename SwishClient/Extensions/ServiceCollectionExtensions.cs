using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwishClient.Clients;
using SwishClient.Config;
using SwishClient.DelegatingHandlers;
using SwishClient.JsonConverters;
using System;
using System.Net.Http;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace SwishClient.Extensions
{
    /// <summary>
    /// This class contains extension methods for IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// This extension method adds Swish api to the service collection.
        /// 
        /// To work it need a SwishConfig SECTION in the configuration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns>The IServiceCollection to allow chaining</returns>
        public static IServiceCollection AddSwishClient(this IServiceCollection services, IConfiguration configuration)
        {
            // Read the SwishConfig from the configuration to make sure we have what we need
            var swishConfig = configuration.GetRequiredSection("SwishConfig").Get<SwishConfig>();

            // Load the client certificate to make sure we have it
            var clientCertificate = new X509Certificate2(
                swishConfig.ClientCertificateFilename,
                swishConfig.ClientCertificatePassword,
                X509KeyStorageFlags.UserKeySet
            );

            // Add the SwishConfig to the service collection
            services.AddSingleton(swishConfig);

            // Add delegating handlers used by the Swish Client
            services
                .AddScoped<HttpLoggingHandler>();

            //var baseUrl = "https://cpc.getswish.net"; // Prod
            var baseUrl = "https://mss.cpc.getswish.net"; // MSS

            var userAgent = $"SwishClient/{Assembly.GetExecutingAssembly().GetName().Version} (Made by Solvation AB)";

            // Add the Swish Payment Client
            services.AddHttpClient<ISwishPaymentClient, SwishPaymentClient>()
                .ConfigureHttpClient(httpClient =>
                {
                    httpClient.BaseAddress = new Uri($"{baseUrl}/swish-cpcapi/");

                    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
                }) 
                .ConfigurePrimaryHttpMessageHandler(sp =>
                {
                    var handler = new HttpClientHandler();

                    handler.SslProtocols = SslProtocols.Tls12;

                    handler.ClientCertificates.Add(clientCertificate);

                    return handler;
                })
                .AddHttpMessageHandler<HttpLoggingHandler>();

            // Add the Swish QrCode Client
            services.AddHttpClient<ISwishQrCodeClient, SwishQrCodeClient>()
                .ConfigureHttpClient(httpClient =>
                {
                    httpClient.BaseAddress = new Uri($"{baseUrl}/qrg-cpcapi/");

                    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
                }) 
                .ConfigurePrimaryHttpMessageHandler(sp =>
                {
                    var handler = new HttpClientHandler();

                    handler.SslProtocols = SslProtocols.Tls12;

                    handler.ClientCertificates.Add(clientCertificate);

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