using glimpse.Data;
using glimpse.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace glimpse.Models.Repository
{
    public class RequestResponseRepository : IRequestResponseRepository
    {
        private readonly DataContext _context;

        public RequestResponseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task Add(RequestResponse requestResponse)
        {
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(requestResponse, 
                new ValidationContext(requestResponse, null, null), results, true);

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

        public async Task<RequestResponse> GetRequestResponse(Guid Id)
        {
            return _context.RequestResponses.FirstOrDefault(x => x.Id.Equals(Id));
        }

        public async Task<IEnumerable<RequestResponse>> GetRequestResponses(Guid? companyId)
        {
            if(companyId.HasValue)
            {
                return _context.RequestResponses.Where(x => x.CompanyId.Equals(companyId)).ToArray();
            } else
            {
                return _context.RequestResponses.ToArray();
            }
        }
    }
}