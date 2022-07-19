using ID.eShop.API.Common.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ID.eShop.API.Common.Extensions
{
    public static class HttpClientExtension
    {
        public static IHttpClientBuilder AddCustomHttpClient<TInterface, TImplementation>(this IServiceCollection services, HttpClientSettings settings)
            where TImplementation : class, TInterface
            where TInterface : class
        {
            if (settings.Timeout < 90)
            {
                throw new ArgumentException($"Timeout too small {settings.Timeout}. Client={typeof(TImplementation)}, Interface={typeof(TInterface)}");
            }

            return services.CreateHttpClientBuilder<TInterface, TImplementation>(settings, name: typeof(TInterface).FullName);
        }

        private static IHttpClientBuilder CreateHttpClientBuilder<TInterface, TImplementation>(this IServiceCollection services, HttpClientSettings settings, string name = null)
            where TImplementation : class, TInterface
            where TInterface : class
        {
            var httpClientBuilder = services.AddHttpClient<TInterface, TImplementation>(
                client => CreateHttpClient(client, settings));

            return httpClientBuilder
                .AddDelegatingHandlers(settings)
                .AddHttpTimeoutHandler(settings);
        }

        private static void CreateHttpClient(HttpClient client, HttpClientSettings settings)
        {
            if (!string.IsNullOrWhiteSpace(settings.BaseUrl))
            {
                client.BaseAddress = new Uri(settings.BaseUrl);
            }

            client.DefaultRequestHeaders.Add(Constants.ApiHeaders.ACCEPT, Constants.ContentTypes.ApplicationJson);

            // timeout must be > 0 or an exception will be thrown
            client.Timeout = TimeSpan.FromMilliseconds(settings.Timeout);
        }

        private static IHttpClientBuilder AddDelegatingHandlers(this IHttpClientBuilder httpClientBuilder, HttpClientSettings settings)
        {
            httpClientBuilder.AddHttpMessageHandler(x => new SourceHeaderDelegatingHandler());
            return httpClientBuilder;
        }

        private static IHttpClientBuilder AddHttpTimeoutHandler(this IHttpClientBuilder httpClientBuilder, HttpClientSettings settings)
        {
            if (settings.Timeout > 0)
            {
                httpClientBuilder.AddHttpMessageHandler(
                    handler => new TimingOutDelegatingHandler { TimeOut = TimeSpan.FromMilliseconds(settings.Timeout) });
            }

            return httpClientBuilder;
        }

        private static IHttpClientBuilder AddRetryHandler(this IHttpClientBuilder httpClientBuilder, HttpClientSettings settings)
        {
            if (settings.MaxRetryCount > 0)
            {
                httpClientBuilder.AddHttpMessageHandler(
                    handler => new RetryPolicyDelegatingHandler(RetryBehavior.NoRetryFor404, settings.MaxRetryCount));
            }

            return httpClientBuilder;
        }
    }
}
