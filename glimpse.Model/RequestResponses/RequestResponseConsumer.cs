using glimpse.Entities;
using glimpse.Models.Queue;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace glimpse.Models.RequestResponses
{
    public class RequestResponseConsumer : IConsumer
    {
        private readonly ChannelReader<RequestResponse> _reader;
        private readonly ILogger<RequestResponseConsumer> _logger;
        private readonly int _instanceId;

        public RequestResponseConsumer(ChannelReader<RequestResponse> reader, 
            ILogger<RequestResponseConsumer> logger, int instanceId)
        {
            _reader = reader;
            _instanceId = instanceId;
            _logger = logger;
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