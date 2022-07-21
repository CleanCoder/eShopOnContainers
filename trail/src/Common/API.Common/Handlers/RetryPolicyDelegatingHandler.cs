using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ID.eShop.API.Common.Handlers
{

    public enum RetryBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        Default = 0,

        /// <summary>
        /// 
        /// </summary>
        NoRetryFor404 = 1,
    }

    public class RetryPolicyDelegatingHandler : DelegatingHandler
    {
        private const int MaxAmountOfRetries = 3;

        private readonly RetryBehavior _retryBehavior;
        private readonly int _maxRetry;

        public RetryPolicyDelegatingHandler(RetryBehavior retryBehavior = RetryBehavior.Default, int maxRetry = MaxAmountOfRetries)
        {
            _retryBehavior = retryBehavior;
            _maxRetry = maxRetry <= 0 ? MaxAmountOfRetries : maxRetry;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Processing);
            response.Content = new StringContent(string.Empty);
            for (int i = 0; i < _maxRetry; i++)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    response = await base.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }

                    // retry 404
                    if (response.StatusCode == HttpStatusCode.NotFound && !_retryBehavior.HasFlag(RetryBehavior.NoRetryFor404))
                    {
                        continue;
                    }

                    // client side errors status code will not retry, 4..
                    if (response.StatusCode >= HttpStatusCode.BadRequest
                        && response.StatusCode < HttpStatusCode.InternalServerError)
                    {
                        return response;
                    }
                }
            }

            return response;
        }
    }
}
