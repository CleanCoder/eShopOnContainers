using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ID.eShop.API.Common.Handlers
{
    public class TimingOutDelegatingHandler : DelegatingHandler
    {
        public TimeSpan TimeOut { get; set; } = TimeSpan.FromMilliseconds(3000);

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                linkedCancellationTokenSource.CancelAfter(TimeOut);

                try
                {
                    return base.SendAsync(request, cancellationToken);
                }
                catch (OperationCanceledException ex)
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        throw new TimeoutException("The request time out", ex);
                    }

                    throw;
                }
            }
        }
    }
}
