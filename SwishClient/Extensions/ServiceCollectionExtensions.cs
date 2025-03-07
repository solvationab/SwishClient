using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwishClient.Clients;
using SwishClient.Config;
using SwishClient.DelegatingHandlers;
using SwishClient.JsonConverters;
using System;
using System.Net.Http;
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

            // Add the Swish API
            services.AddHttpClient<IPaymentClient, PaymentClient>()
                //.ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri("https://cpc.getswish.net/swish-cpcapi/")) // Prod
                .ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri("https://mss.cpc.getswish.net/swish-cpcapi/")) // MSS
                .ConfigurePrimaryHttpMessageHandler(sp =>
                {
                    var handler = new HttpClientHandler();

                    //var certBytes = File.ReadAllBytes(swishConfig.ClientCertificateFilename);

                    //SetCertificate(handler, certBytes, swishConfig.ClientCertificatePassword);

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

        private static void SetCertificate(HttpClientHandler handler, byte[] certBytes, string password)
        {
            var certs = new X509Certificate2Collection();

            certs.Import(certBytes, password, X509KeyStorageFlags.UserKeySet);

            foreach (var cert in certs)
            {
                if (cert.HasPrivateKey)
                {
                    handler.ClientCertificates.Add(cert);
                }
                else
                {
                    //Add the intermediate certificate to the trusted root store
                    //which acts as a cache during the TLS handshake
                    using (var store = new X509Store(StoreName.CertificateAuthority, StoreLocation.CurrentUser))
                    {
                        store.Open(OpenFlags.ReadWrite);
                        store.Add(cert);
                    }
                }
            }
        }
    }
}