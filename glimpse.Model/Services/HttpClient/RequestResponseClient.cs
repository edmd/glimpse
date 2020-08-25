using System.Net.Http;

namespace glimpse.Models.Services
{
    public class RequestResponseClient
    {
        public HttpClient Client { get; }

        public RequestResponseClient(HttpClient httpClient)
        {
            Client = httpClient;
        }
    }
}
