using System;
using System.Threading;
using System.Threading.Tasks;

namespace glimpse_data.Models.Messaging
{
    public interface IConsumer
    {
        Task BeginConsumeAsync(CancellationToken cancellationToken = default);
    }
}