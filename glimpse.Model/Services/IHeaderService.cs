using glimpse.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace glimpse.Models
{
    public interface IHeaderService
    {
        Task<List<Header>> GetHeaders(Guid? requestHeaderGroupId, Guid? responseHeaderGroupId);

        Task<Header> GetHeader(Guid id);

        Task InsertHeader(Header header);

        Task<Header> DeleteHeader(Guid id);
    }
}
