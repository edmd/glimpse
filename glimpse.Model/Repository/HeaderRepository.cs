using glimpse.Data;
using glimpse.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace glimpse.Models
{
    public class HeaderRepository : IHeaderRepository
    {
        private readonly DataContext _context;

        public HeaderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Header>> GetHeaders(bool requestHeaders = true)
        {
            var list = (List<Header>)null;

            list = _context.Headers.Where(x => x.IsRequestHeader == requestHeaders).ToList();
            
            return list;
        }

        public async Task<Header> GetHeader(Guid id)
        {
            var header = _context.Headers.FirstOrDefault(i => i.Id == id);

            return header;
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

        public async Task<Header> DeleteHeader(Guid id)
        {
            var header = _context.Headers.Find(id);
            if (header != null)
            {
                _context.Headers.Remove(header);
                await _context.SaveChangesAsync();
            }

            return header;
        }
    }
}