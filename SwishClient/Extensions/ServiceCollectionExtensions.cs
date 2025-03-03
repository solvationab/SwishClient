﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using SwishClient.JsonConverters;
using System;
using System.Text.Json;
using SwishClient.DelegatingHandlers;

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
            services
                .AddRefitClient<ISwishClient>(sp =>
                {
                    var settings = new RefitSettings
                    {
                        ContentSerializer = new SystemTextJsonContentSerializer(GetJsonSerializerOptions())
                    };

                    //settings.ExceptionFactory = async responseMessage =>
                    //{
                    //    if (responseMessage.IsSuccessStatusCode)
                    //        return null;

                    //    var SwishError = await responseMessage.Content.ReadFromJsonAsync<SwishErrorResponse>();

                    //    if (SwishError?.ErrorInformation != null)
                    //    {
                    //        return new SwishException(SwishError);
                    //    }

                    //    var requestMessage = responseMessage.RequestMessage;

                    //    var method = requestMessage.Method;

                    //    return await ApiException
                    //        .Create(requestMessage, method, responseMessage, settings);
                    //};

                    return settings;
                })
                .ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri("https://api.Swish.se"))
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