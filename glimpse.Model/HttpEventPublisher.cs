using glimpse.Entities;
using glimpse.Models.Queue;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Timers;

namespace glimpse.Models
{
    /// <summary>
    /// An HttpEventPublisher is instantiated for each instance of RequestResponse. 
    /// It pushes messages to the RabbitPublisher
    /// </summary>
    public class HttpEventPublisher : IDisposable
    {
        private Timer _timer;
        private RequestResponse _requestResponse;

        private const string ExchangeName = "HttpEvent";

        private readonly IBusConnection _connection;
        private IModel _channel;
        private readonly IBasicProperties _properties;

        public HttpEventPublisher(IBusConnection connection, RequestResponse requestResponse)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _channel = _connection.CreateChannel();
            _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Fanout);
            _properties = _channel.CreateBasicProperties();

            _requestResponse = requestResponse;

            _timer = new Timer(requestResponse.Interval);
            _timer.Elapsed += RaiseHttpEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void RaiseHttpEvent(Object source, ElapsedEventArgs e)
        {
            var jsonData = JsonSerializer.Serialize(_requestResponse);

            var body = Encoding.UTF8.GetBytes(jsonData);

            _channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: string.Empty,
                mandatory: true,
                basicProperties: _properties,
                body: body);

            Console.WriteLine("Raised: {0}", e.SignalTime);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _channel = null;
        }
    }
}