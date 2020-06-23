using glimpse.Models.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Threading.Tasks;

namespace glimpse.Models
{
    public class RequestService : IRequestService
    {
        private readonly DataContext _context;

        public RequestService(DataContext context)
        {
            _context = context;
        }

        public Task<List<RequestResponse>> GetRequestResponse()
        {
            var list = _context.RequestResponses
                .Include(x => x.RequestHeaders)
                .Include(x => x.ResponseHeaders)
                .ToListAsync();

            return list;
        }

        public Task<RequestResponse> GetRequestResponse(Guid id)
        {
            var requestResponse = _context.RequestResponses
                .Include(x => x.RequestHeaders)
                .Include(x => x.ResponseHeaders)
                .FirstOrDefaultAsync(i => i.Id == id);

            return requestResponse;
        }

        public async Task InsertRequestResponse(RequestResponse requestResponse)
        {
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(requestResponse, new ValidationContext(requestResponse, null, null),
                results, true);

            if (isValid)
            {

                _context.RequestResponses.Add(requestResponse);
                await _context.SaveChangesAsync();
            }
            else
            {
                await Task.FromException(null); // Testing this out in the tests
            }
        }

        public Task<RequestResponse> DeleteRequestResponse(Guid id)
        {
            var requestResponse = _context.RequestResponses.Find(id);
            if (requestResponse != null)
            {
                if (requestResponse.IsActive)
                {
                    throw new Exception($"RequestResponse '{id}' is active, cannot be deleted");
                }

                _context.RequestResponses.Remove(requestResponse);
            }

            return Task.FromResult(requestResponse);
        }
    }
}
