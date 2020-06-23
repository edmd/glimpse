using glimpse.Models.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace glimpse.Models
{
    public class HeaderService : IHeaderService
    {
        private readonly DataContext _context;

        public HeaderService(DataContext context)
        {
            _context = context;
        }

        public Task<List<Header>> GetHeaders(Guid? requestHeaderGroupId, Guid? responseHeaderGroupId)
        {
            var list = (List<Header>)null;

            if(requestHeaderGroupId != null && requestHeaderGroupId != Guid.Empty)
            {
                list = _context.Headers.Where(x => x.RequestHeaderGroupId == requestHeaderGroupId).ToList();
            } else
            {
                list = _context.Headers.Where(x => x.ResponseHeaderGroupId == responseHeaderGroupId).ToList();
            }

            return Task.FromResult(list);
        }

        public Task<Header> GetHeader(Guid id)
        {
            var header = _context.Headers.FirstOrDefault(i => i.HeaderId == id);

            return Task.FromResult(header);
        }

        public async Task InsertHeader(Header header)
        {
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(header, new ValidationContext(header, null, null),
                results, true);

            if (isValid)
            {

                _context.Headers.Add(header);
                await _context.SaveChangesAsync();
            }
            else
            {
                await Task.FromException(null); // Testing this out in the tests
            }
        }

        public Task<Header> DeleteHeader(Guid id)
        {
            var header = _context.Headers.Find(id);
            if (header != null)
            {
                _context.Headers.Remove(header);
            }

            return Task.FromResult(header);
        }
    }
}