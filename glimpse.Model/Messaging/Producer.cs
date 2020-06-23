using System.Threading.Tasks;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace glimpse.Models.Messaging
{
    public class Producer : IProducer
    {
        private readonly ChannelWriter<RequestResponse> _writer;
        private readonly ILogger<Producer> _logger;

        public Producer(ChannelWriter<RequestResponse> writer, ILogger<Producer> logger)
        {
            _writer = writer;
            _logger = logger;
        }

        public async Task PublishAsync(RequestResponse requestResponse, CancellationToken cancellationToken = default)
        {
            await _writer.WriteAsync(requestResponse, cancellationToken);
        }
    }
}