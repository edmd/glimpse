using RabbitMQ.Client.Events;

namespace glimpse.Models.Messaging
{
    public interface ISubscriber
    {
        void Start();
        event AsyncEventHandler<RabbitSubscriberEventArgs> OnMessage;
    }
}
