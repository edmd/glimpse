using glimpse.Models.HttpEvent;
using System.Collections.Generic;

namespace glimpse.Models.Repository
{
    public interface IHttpResponseEventRepository
    {
        void Add(HttpResponseEvent responseEvent);
        IReadOnlyCollection<HttpResponseEvent> GetEvents();
    }
}