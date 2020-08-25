using System.Threading.Tasks;
using System.Threading;
using glimpse.Entities;

namespace glimpse.Models.RequestResponses
{
    public interface IProducer
    {
        Task PublishAsync(RequestResponse requestResponse, CancellationToken cancellationToken = default);
    }
}