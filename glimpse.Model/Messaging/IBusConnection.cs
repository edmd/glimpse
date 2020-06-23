using RabbitMQ.Client;

namespace glimpse.Models.Messaging
{
    public interface IBusConnection
    {
        bool IsConnected { get; }

        IModel CreateChannel();
    }
}