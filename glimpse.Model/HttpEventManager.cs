using glimpse.Models.Messaging;
using glimpse.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace glimpse.Models
{
    /// <summary>
    /// Load and instantiate the HttpEvents
    /// </summary>
    public class HttpEventManager : IHttpEventManager
    {
        // We need to refresh the list every 5 minutes
        private DataContext _context;
        private RabbitPublisher _publisher;

        public List<HttpEventPublisher> Publishers { get; }

        public HttpEventManager(DataContext context, RabbitPublisher publisher)
        {
            _context = context;
            _publisher = publisher; // Sharing single Rabbit Queue with all publishers - in future we may need to split this out
            Publishers = new List<HttpEventPublisher>();

            if (_context.RequestResponses.Any())
            {
                foreach (var requestResponse in _context.RequestResponses.ToArray())
                {
                    Publishers.Add(new HttpEventPublisher(requestResponse, _publisher));
                }
            } else
            {
                // Populate with test data
                Publishers.Add(TestData());
            }
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public HttpEventPublisher TestData()
        {
            var requestResponse = new RequestResponse
            {
                Id = Guid.NewGuid(),
                Interval = 5000,
                IsActive = true,
                Method = HttpMethod.Get,
                RequestBody = string.Empty,
                RequestHeaderGroupId = Guid.NewGuid(),
                ResponseHeaderGroupId = Guid.NewGuid(),
                ResponseStatus = System.Net.HttpStatusCode.OK,
                Url = new Uri("www.google.co.uk")
            };

            requestResponse.AddRequestHeader("Host", "www.google.co.uk");
            requestResponse.AddRequestHeader("Accept", "text/html");
            requestResponse.AddResponseHeader("Content-Type", "text/html; charset=ISO-8859-1");

            return new HttpEventPublisher(requestResponse, _publisher);
        }
    }
}