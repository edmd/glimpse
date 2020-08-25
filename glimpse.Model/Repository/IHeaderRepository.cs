using glimpse.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace glimpse.Models
{
    public interface IHeaderRepository
    {
        Task<List<Header>> GetHeaders(bool requestHeaders);

        Task<Header> GetHeader(Guid id);

        Task InsertHeader(Header header);

        Task<Header> DeleteHeader(Guid id);
    }
}