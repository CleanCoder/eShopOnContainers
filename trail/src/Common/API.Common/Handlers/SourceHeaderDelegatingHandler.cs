using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ID.eShop.API.Common.Handlers
{
    public class SourceHeaderDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(Constants.ApiHeaders.Source))
                request.Headers.Add(Constants.ApiHeaders.Source, AppDomain.CurrentDomain.FriendlyName);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
