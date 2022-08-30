using EncryptionExample.Model;
using Microsoft.EntityFrameworkCore;

namespace EncryptionExample.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Food> Foods { get; set; }
    }
}
