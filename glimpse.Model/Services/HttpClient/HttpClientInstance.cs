using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace glimpse_data.Models
{
    // Request headers, body, url, method, (delay?)
    // Pass in expected response headers, httpstatus and body
    public class HttpClientInstance
    {
        private readonly IHttpClientFactory _clientFactory;

        private readonly ICollection<Header> _requestHeaders;
        private readonly Uri _url;
        private readonly HttpMethod _method;
        private readonly string _requestBody;

        private readonly HttpStatusCode _responseStatus;
        private readonly ICollection<Header> _responseHeaders;
        private readonly string _responseBody;

        private readonly int _acceptableThreshold;

        public HttpClientInstance(IHttpClientFactory clientFactory, RequestResponse requestResponse, 
            int acceptableThreshold = 60)
        {
            _clientFactory = clientFactory;

            _requestHeaders = requestResponse.RequestHeaders;
            _url = requestResponse.Url;
            _method = requestResponse.Method;
            _requestBody = requestResponse.RequestBody;

            _responseStatus = requestResponse.ResponseStatus;
            _responseHeaders = requestResponse.ResponseHeaders;
            _responseBody = requestResponse.ResponseBody;

            _acceptableThreshold = acceptableThreshold;
        }

        public async Task OnGet()
        {
            // Generate cancellation tokens
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;


            var request = new HttpRequestMessage(_method, _url);

            if (_requestHeaders != null)
            {
                foreach (var item in _requestHeaders)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            if(_requestBody != null)
            {
                var content = new StringContent(_requestBody);
                request.Content = content;
            }

            var client = _clientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(_acceptableThreshold);
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead, token);

            if(response.StatusCode != _responseStatus)
            {
                // Raise Api monitoring error event
            }

            var responseString = await response.Content.ReadAsStringAsync();
            
            if(string.Compare(responseString, _responseBody) != 0)
            {
                // Raise Api monitoring error event
            }

            var responseHeaders = response.Headers;

            // check only the values we're interested in
            foreach(var header in _responseHeaders)
            {
                if (string.Compare(responseHeaders.GetValues(header.Key).FirstOrDefault(), header.Value) != 0)
                {
                    // Raise Api monitoring error event
                }
            }
        }
    }
}