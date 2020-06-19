using RabbitMQ.Client;

namespace glimpse_data.Models.Messaging
{
    public interface IBusConnection
    {
        bool IsConnected { get; }

        IModel CreateChannel();
    }
}