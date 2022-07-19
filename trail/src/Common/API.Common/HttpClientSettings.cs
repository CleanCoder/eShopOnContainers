using System;
using System.Collections.Generic;
using System.Text;

namespace ID.eShop.API.Common
{
    public class HttpClientSettings
    {
        public string BaseUrl { get; set; }

        public int MaxRetryCount { get; set; } = 0;  // By default: no retry


        public int Timeout { get; set; } = 5000;

        public bool? UseAuthorizationHeaderInjectionHandler { get; set; }

    }
}
