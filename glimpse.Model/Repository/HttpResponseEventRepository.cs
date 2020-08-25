using glimpse.Data;
using glimpse.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace glimpse.Models.Repository
{
    public class HttpResponseEventRepository : IHttpResponseEventRepository
    {
        private readonly DataContext _context;

        public HttpResponseEventRepository(DataContext context)
        {
            _context = context;
        }

        public async Task Add(HttpResponseEvent httpResponseEvent)
        {
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(httpResponseEvent,
                new ValidationContext(httpResponseEvent, null, null), results, true);

            if (isValid)
            {
                _context.DetachLocal(httpResponseEvent.RequestResponse, httpResponseEvent.RequestResponse.Id);
                _context.HttpResponseEvents.Add(httpResponseEvent);
                await _context.SaveChangesAsync();
            }
            else
            {
                await Task.FromException(null); // Testing this out in the tests
            }
        }

        public async Task<IEnumerable<HttpResponseEvent>> GetEvents(Guid? requestResponseId)
        {
            if (requestResponseId.HasValue)
            {
                return _context.HttpResponseEvents.Where(x => x.RequestResponse.Id.Equals(requestResponseId.Value)).ToArray();
            } else
            {
                return _context.HttpResponseEvents.ToArray();
            }
        }
    }
}