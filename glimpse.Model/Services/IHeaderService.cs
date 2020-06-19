using glimpse_data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace glimpse_data.Models
{
    public interface IHeaderService
    {
        Task<List<Header>> GetHeaders(Guid? groupId);

        Task<Header> GetHeader(Guid id);

        Task InsertHeader(Header header);

        Task<Header> DeleteHeader(Guid id);
    }
}
