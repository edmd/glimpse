using System.Threading.Tasks;
using System.Threading;

namespace glimpse.Models.Messaging
{
    public interface IProducer
    {
        Task PublishAsync(RequestResponse requestResponse, CancellationToken cancellationToken = default);
    }
}
