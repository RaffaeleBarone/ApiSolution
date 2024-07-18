using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;

namespace JsonPlaceholderWebApi.Extensions
{
    public static class HttpClientExtensions
    {
        public static IHttpClientBuilder AddRetryPolicy(this IHttpClientBuilder builder)
        {
            return builder.AddPolicyHandler(GetRetryPolicy());
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(15)
                });
        }
    }
}
