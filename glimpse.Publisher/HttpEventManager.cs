using glimpse.Models;
using glimpse.Models.Messaging;
using glimpse.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace glimpse.Publisher
{
    /// <summary>
    /// Load and instantiate the HttpEvents
    /// </summary>
    public class HttpEventManager
    {
        private static DataContext _context;

        public static List<HttpEventPublisher> Publishers { get; set; }

        public HttpEventManager(DataContext context, IBusConnection connection)
        {
            _context = context;
            Publishers = new List<HttpEventPublisher>();

            if (_context != null && _context.RequestResponses.Any())
            {
                foreach (var requestResponse in _context.RequestResponses.ToArray())
                {
                    Publishers.Add(new HttpEventPublisher(connection, requestResponse));
                }
            } else
            {
                // Populate with test data
                Publishers.Add(new HttpEventPublisher(connection, TestData()));
            }
        }

        public void ForceRefresh()
        {
            throw new NotImplementedException();
        }

        public static RequestResponse TestData()
        {
            var requestResponse = new RequestResponse
            {
                Id = Guid.NewGuid(),
                Interval = 10000,
                IsActive = true,
                Method = HttpMethod.Get.Method,
                RequestBody = string.Empty,
                RequestHeaderGroupId = Guid.NewGuid(),
                ResponseHeaderGroupId = Guid.NewGuid(),
                ResponseStatus = System.Net.HttpStatusCode.OK,
                Url = new Uri("https://www.google.co.uk")
            };

            requestResponse.AddRequestHeader("Host", "www.google.co.uk", requestResponse.RequestHeaderGroupId);
            requestResponse.AddRequestHeader("Accept", "text/html", requestResponse.RequestHeaderGroupId);
            //requestResponse.AddResponseHeader("Content-Type", "text/html; charset=ISO-8859-1", requestResponse.ResponseHeaderGroupId);

            return requestResponse;
        }
    }
}