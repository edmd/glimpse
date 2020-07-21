using glimpse.Models.HttpEvent;
using System;
using System.Collections.Generic;

namespace glimpse.Models.Repository
{
    public class HttpResponseEventRepository : IHttpResponseEventRepository
    {
        private readonly Queue<HttpResponseEvent> _httpResponseEvent;

        public HttpResponseEventRepository()
        {
            _httpResponseEvent = new Queue<HttpResponseEvent>();
        }

        public void Add(HttpResponseEvent httpResponseEvent)
        {
            _httpResponseEvent.Enqueue(httpResponseEvent ?? throw new ArgumentNullException(nameof(httpResponseEvent)));
        }

        public IReadOnlyCollection<HttpResponseEvent> GetEvents()
        {
            return _httpResponseEvent.ToArray();
        }
    }
}