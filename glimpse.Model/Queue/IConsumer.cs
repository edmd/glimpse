using System;
using System.Threading;
using System.Threading.Tasks;

namespace glimpse.Models.Queue
{
    public interface IConsumer
    {
        Task BeginConsumeAsync(CancellationToken cancellationToken = default);
    }
}