using glimpse.Entities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace glimpse.Models
{
    public class HttpClientInstance : IHttpClientInstance
    {
        private readonly HttpClient _httpClient;

        public HttpClientInstance(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Tuple<RequestResponse, HttpResponseEvent>> OnGet(RequestResponse requestResponse)
        {
            var _requestResponse = requestResponse;
            var _httpResponseEvent = new HttpResponseEvent();
            var stopWatch = new Stopwatch();
            _httpResponseEvent.RequestResponse = requestResponse;
            _httpResponseEvent.Url = requestResponse.Url;

            try
            {
                // Generate cancellation tokens
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;

                var request = new HttpRequestMessage(new HttpMethod(requestResponse.Method), requestResponse.Url);

                if (requestResponse.Headers != null)
                {
                    foreach (var item in requestResponse.Headers.Where(x => x.IsRequestHeader == true))
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }

                if (!string.IsNullOrEmpty(requestResponse.RequestBody))
                {
                    var content = new StringContent(requestResponse.RequestBody);
                    request.Content = content;
                }

                stopWatch.Start();
                _httpResponseEvent.StartDate = DateTime.Now;
                _httpResponseEvent.ResponseType = HttpResponseType.Green;

                var response = await _httpClient.SendAsync(request, CancellationToken.None);

                stopWatch.Stop();
                _httpResponseEvent.ElapsedTime = stopWatch.Elapsed;
                if(requestResponse.AcceptableResponseTimeMs < _httpResponseEvent.ElapsedTime.TotalMilliseconds)
                {
                    _httpResponseEvent.ResponseType = HttpResponseType.Amber;
                }

                _requestResponse.ResponseStatus = response.StatusCode;
                if (response.StatusCode != requestResponse.ResponseStatus)
                {
                    // Raise Api monitoring error event
                    _httpResponseEvent.ResponseType = HttpResponseType.Red;
                }

                var responseString = await response.Content.ReadAsStringAsync();
                _requestResponse.ResponseBody = responseString;

                if (string.Compare(responseString, requestResponse.ResponseBody) != 0)
                {
                    // Raise Api monitoring error event
                    _httpResponseEvent.ResponseType = HttpResponseType.Red;
                }

                if (requestResponse.Headers != null)
                {
                    // check only the values we're interested in
                    foreach (var header in requestResponse.Headers.Where(x => x.IsRequestHeader == false))
                    {
                        _requestResponse.Headers.Add(header);
                        var responseHeader = response.Headers.GetValues(header.Key).FirstOrDefault();
                        if (!string.IsNullOrEmpty(responseHeader) && string.Compare(responseHeader, header.Value) != 0)
                        {
                            // Raise Api monitoring error event
                            _httpResponseEvent.ResponseType = HttpResponseType.Red;
                        }
                    }
                }

                return new Tuple<RequestResponse, HttpResponseEvent>(_requestResponse, _httpResponseEvent);
            } catch(Exception ex)
            {
                stopWatch.Stop();
                _httpResponseEvent.ElapsedTime = stopWatch.Elapsed;

                var message = ex.Message;
            }

            return new Tuple<RequestResponse, HttpResponseEvent>(
                _requestResponse, _httpResponseEvent);
        }
    }
}