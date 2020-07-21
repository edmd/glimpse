using System.Threading.Tasks;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using System.Threading;
using glimpse.Models.Repository;

namespace glimpse.Models.Messaging
{
    public class Producer : IProducer
    {
        private readonly ChannelWriter<RequestResponse> _writer;
        private readonly ILogger<Producer> _logger;
        private readonly IHttpClientInstance _httpClientInstance;
        private readonly IRequestResponseRepository _messagesRepository;
        private readonly IHttpResponseEventRepository _eventsRepository;

        public Producer(ChannelWriter<RequestResponse> writer, 
            ILogger<Producer> logger, IHttpClientInstance httpClientInstance, 
            IRequestResponseRepository messagesRepository,
            IHttpResponseEventRepository eventsRepository)
        {
            _httpClientInstance = httpClientInstance;
            _messagesRepository = messagesRepository;
            _eventsRepository = eventsRepository;
            _writer = writer;
            _logger = logger;
        }

        public async Task PublishAsync(RequestResponse requestResponse, CancellationToken cancellationToken = default)
        {
            var returnedTuple = await _httpClientInstance.OnGet(requestResponse);
            _messagesRepository.Add(returnedTuple.Item1);
            _eventsRepository.Add(returnedTuple.Item2);

            await _writer.WriteAsync(returnedTuple.Item1, cancellationToken);
        }
    }
}