using System.Collections.Generic;

namespace glimpse_data.Models.Repository
{
    public interface IRequestResponseRepository
    {
        void Add(RequestResponse message);
        IReadOnlyCollection<RequestResponse> GetMessages();
    }
}