using glimpse.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace glimpse.Models
{
    public interface IRequestService
    {
        Task<List<RequestResponse>> GetRequestResponse();

        Task<RequestResponse> GetRequestResponse(Guid id);

        Task InsertRequestResponse(RequestResponse request);

        Task<RequestResponse> DeleteRequestResponse(Guid id);
    }
}
