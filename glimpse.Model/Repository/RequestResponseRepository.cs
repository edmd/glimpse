using System;
using System.Collections.Generic;

namespace glimpse_data.Models.Repository
{
    public class RequestResponseRepository : IRequestResponseRepository
    {
        private readonly Queue<RequestResponse> _messages;

        public RequestResponseRepository()
        {
            _messages = new Queue<RequestResponse>();
        }

        public void Add(RequestResponse requestResponse)
        {
            _messages.Enqueue(requestResponse ?? throw new ArgumentNullException(nameof(requestResponse)));
        }

        public IReadOnlyCollection<RequestResponse> GetMessages()
        {
            return _messages.ToArray();
        }
    }
}