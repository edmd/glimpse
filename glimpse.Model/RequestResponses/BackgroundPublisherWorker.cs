using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace glimpse.Models.RequestResponses
{
    public class BackgroundPublisherWorker : BackgroundService
    {
        private readonly IHttpEventManager _httpEventManager;
        private readonly ILogger<BackgroundPublisherWorker> _logger;

        public BackgroundPublisherWorker(IHttpEventManager httpEventManager, ILogger<BackgroundPublisherWorker> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpEventManager = httpEventManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _httpEventManager.PopulatePublishers();
        }
    }
}