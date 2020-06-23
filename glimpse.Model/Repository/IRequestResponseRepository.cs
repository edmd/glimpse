using System.Collections.Generic;

namespace glimpse.Models.Repository
{
    public interface IRequestResponseRepository
    {
        void Add(RequestResponse message);
        IReadOnlyCollection<RequestResponse> GetMessages();
    }
}