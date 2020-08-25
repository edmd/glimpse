using glimpse.Entities;
using glimpse.Models.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace glimpse.Models.RequestResponses
{
    public class RequestResponseProducer : IProducer
    {
        private readonly ChannelWriter<RequestResponse> _writer;
        private readonly ILogger<RequestResponseProducer> _logger;
        private readonly IHttpClientInstance _httpClientInstance;
        private readonly IHttpResponseEventRepository _eventsRepository;

        public RequestResponseProducer(ChannelWriter<RequestResponse> writer, 
            ILogger<RequestResponseProducer> logger, IHttpClientInstance httpClientInstance, 
            IHttpResponseEventRepository eventsRepository)
        {
            _httpClientInstance = httpClientInstance;
            _eventsRepository = eventsRepository;
            _writer = writer;
            _logger = logger;
        }

        public async Task PublishAsync(RequestResponse requestResponse, CancellationToken cancellationToken = default)
        {
            try
            {
                var returnedTuple = await _httpClientInstance.OnGet(requestResponse);
                _eventsRepository.Add(returnedTuple.Item2);

                await _writer.WriteAsync(returnedTuple.Item1, cancellationToken);
            } catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message, requestResponse);
            }
        }
    }
}