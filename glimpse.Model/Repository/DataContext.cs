using glimpse_data.Models;
using Microsoft.EntityFrameworkCore;

namespace glimpse_data.Models.Repository
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<RequestResponse> RequestResponses { get; set; }

        public DbSet<Header> Headers { get; set; }
    }
}