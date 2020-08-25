using glimpse.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace glimpse.Models.Repository
{
    public interface IHttpResponseEventRepository
    {
        Task Add(HttpResponseEvent responseEvent);
        Task<IEnumerable<HttpResponseEvent>> GetEvents(Guid? requestResponseId);
    }
}