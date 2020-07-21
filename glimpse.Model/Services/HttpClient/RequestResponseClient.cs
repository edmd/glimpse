using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace glimpse.Models.Services
{
    public class RequestResponseClient
    {
        public HttpClient Client { get; }

        public RequestResponseClient(HttpClient httpClient)
        {
            Client = httpClient;
        }
    }
}
