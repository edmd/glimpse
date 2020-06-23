using System.Threading.Tasks;
using System.Threading.Channels;
using System.Threading;
using System;
using Microsoft.Extensions.Logging;
using glimpse.Models.Repository;

namespace glimpse.Models.Messaging
{
    public class Consumer : IConsumer
    {
        private readonly ChannelReader<RequestResponse> _reader;
        private readonly ILogger<Consumer> _logger;

        private readonly IRequestResponseRepository _messagesRepository;
        private readonly int _instanceId;

        public Consumer(ChannelReader<RequestResponse> reader, ILogger<Consumer> logger, int instanceId, IRequestResponseRepository messagesRepository)
        {
            _reader = reader;
            _instanceId = instanceId;
            _logger = logger;
            _messagesRepository = messagesRepository;
        }

        public async Task BeginConsumeAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Consumer {_instanceId} > starting");

            try
            {
                await foreach (var requestResponse in _reader.ReadAllAsync(cancellationToken))
                {
                    _logger.LogInformation($"CONSUMER ({_instanceId})> Received message {requestResponse.Id}");
                    await Task.Delay(500, cancellationToken);
                    _messagesRepository.Add(requestResponse);
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning($"Consumer {_instanceId} > forced stop");
            }

            _logger.LogInformation($"Consumer {_instanceId} > shutting down");
        }

    }
}