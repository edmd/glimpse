using glimpse.Models.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace glimpse.Models.Messaging
{
    public class Consumer : IConsumer
    {
        private readonly ChannelReader<RequestResponse> _reader;
        private readonly ILogger<Consumer> _logger;

        //private readonly IRequestResponseRepository _messagesRepository;
        private readonly int _instanceId;

        public Consumer(ChannelReader<RequestResponse> reader, 
            ILogger<Consumer> logger, int instanceId
            //, IRequestResponseRepository messagesRepository
            )
        {
            _reader = reader;
            _instanceId = instanceId;
            _logger = logger;
            //_messagesRepository = messagesRepository;
        }

        public async Task BeginConsumeAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Consumer {_instanceId} > starting");

            try
            {
                await foreach (var message in _reader.ReadAllAsync(cancellationToken))
                {
                    _logger.LogInformation($"CONSUMER ({_instanceId})> Received message {message.Id} : {message.Url}");
                    await Task.Delay(500, cancellationToken);
                    //_messagesRepository.Add(message);
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