using RabbitMQ.Client;
using System;
using System.Text.Json;
using System.Text;

namespace glimpse.Models.Messaging
{
    public class RabbitPublisher : IDisposable
    {
        private const string ExchangeName = "HttpEvent"; 
        
        private readonly IBusConnection _connection;
        private IModel _channel;
        private readonly IBasicProperties _properties;

        public RabbitPublisher(IBusConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _channel = _connection.CreateChannel();
            _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Fanout);
            _properties = _channel.CreateBasicProperties();
        }

        public void Publish(RequestResponse requestResponse)
        {
            var jsonData = JsonSerializer.Serialize(requestResponse);

            var body = Encoding.UTF8.GetBytes(jsonData);            

            _channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: string.Empty,
                mandatory: true,
                basicProperties: _properties,
                body: body);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _channel = null;
        }
    }
}