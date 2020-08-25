using glimpse.Entities;
using glimpse.Models.Queue;
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
        private IRequestResponseRepository _requestResponseRepository;
        private IBusConnection _connection;

        public List<HttpEventPublisher> Publishers { get; set; }

        public HttpEventManager(IRequestResponseRepository requestResponseRepository, IBusConnection connection)
        {
            _requestResponseRepository = requestResponseRepository;
            _connection = connection;

            PopulatePublishers();
        }

        public void PopulatePublishers()
        {
            if (_connection != null)
            {
                Publishers = new List<HttpEventPublisher>();
                var requestResponses = _requestResponseRepository.GetRequestResponses(null).Result;
                if (requestResponses.Any())
                {
                    foreach (var requestResponse in requestResponses.ToList())
                    {
                        Publishers.Add(new HttpEventPublisher(_connection, requestResponse));
                    }
                }
                //else
                //{
                //    // Populate with test data
                //    Publishers.Add(new HttpEventPublisher(connection, TestData()));
                //}
            }
        }

        private RequestResponse TestData()
        {
            var requestResponse = new RequestResponse
            {
                AcceptableResponseTimeMs = 500,
                Id = Guid.NewGuid(),
                Interval = 10000,
                IsActive = true,
                Method = HttpMethod.Get.Method,
                RequestBody = string.Empty,
                ResponseStatus = System.Net.HttpStatusCode.OK,
                Url = new Uri("https://www.google.co.uk")
            };

            requestResponse.AddRequestHeader("Host", "www.google.co.uk");
            requestResponse.AddRequestHeader("Accept", "text/html");

            return requestResponse;
        }
    }
}