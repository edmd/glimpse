using RabbitMQ.Client.Events;

namespace glimpse_data.Models.Messaging
{
    public interface ISubscriber
    {
        void Start();
        event AsyncEventHandler<RabbitSubscriberEventArgs> OnMessage;
    }
}
