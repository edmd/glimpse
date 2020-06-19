using System.Threading.Tasks;
using System.Threading;

namespace glimpse_data.Models.Messaging
{
    public interface IProducer
    {
        Task PublishAsync(RequestResponse requestResponse, CancellationToken cancellationToken = default);
    }
}
