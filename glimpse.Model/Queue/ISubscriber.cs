using RabbitMQ.Client.Events;

namespace glimpse.Models.Queue
{
    public interface ISubscriber
    {
        void Start();
        event AsyncEventHandler<RabbitSubscriberEventArgs> OnMessage;
    }
}
