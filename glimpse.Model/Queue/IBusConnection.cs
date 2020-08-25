using RabbitMQ.Client;

namespace glimpse.Models.Queue
{
    public interface IBusConnection
    {
        bool IsConnected { get; }

        IModel CreateChannel();
    }
}