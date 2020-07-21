using glimpse.Models;
using glimpse.Models.HttpEvent;
using Microsoft.EntityFrameworkCore;

namespace glimpse.Models.Repository
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<RequestResponse> RequestResponses { get; set; }

        public DbSet<Header> Headers { get; set; }

        public DbSet<HttpResponseEvent> HttpResponseEvents { get; set; }
    }
}