using glimpse.Entities;
using System;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace glimpse.Models.Queue
{
    public class RabbitSubscriber : ISubscriber, IDisposable
    {
        private readonly IBusConnection _connection;
        private IModel _channel;
        private QueueDeclareOk _queue;

        private const string ExchangeName = "HttpEvent";

        public RabbitSubscriber(IBusConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        private void InitChannel()
        {
            _channel?.Dispose();
            _channel = _connection.CreateChannel();
            _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Fanout);

            // Since we're using a Fanout exchange we let Rabbit generate a 
            // queue name for us. This also means that we need to store 
            // the queue name to be able to consume messages from it
            _queue = _channel.QueueDeclare(queue: string.Empty,
                durable: false,
                exclusive: false,
                autoDelete: true,
                arguments: null);
            _channel.QueueBind(queue: String.Empty, exchange: ExchangeName, routingKey: "");

            _channel.CallbackException += (sender, ea) => {
                InitChannel();
                InitSubscription();
            };
        }

        private void InitSubscription()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            
            consumer.Received += OnMessageReceivedAsync;
            
            _channel.BasicConsume(queue: _queue.QueueName, autoAck: true, consumer: consumer);
        }

        private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            var body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            var requestResponse = JsonSerializer.Deserialize<RequestResponse>(body);
            await this.OnMessage(this, new RabbitSubscriberEventArgs(requestResponse));
        }

        public event AsyncEventHandler<RabbitSubscriberEventArgs> OnMessage;

        public void Start()
        {
            InitChannel();
            InitSubscription();
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}