using glimpse.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace glimpse.Models.Repository
{
    public interface IRequestResponseRepository
    {
        Task Add(RequestResponse requestResponse);
        Task<IEnumerable<RequestResponse>> GetRequestResponses(Guid? CompanyId);
    }
}