using glimpse.Models.HttpEvent;
using System;
using System.Threading.Tasks;

namespace glimpse.Models
{
    public interface IHttpClientInstance
    {
        Task<Tuple<RequestResponse, HttpResponseEvent>> OnGet(RequestResponse requestResponse);
    }
}