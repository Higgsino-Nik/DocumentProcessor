using DocumentProcessor.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace DocumentProcessor.Configuration
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<DalDocument> Documents { get; set; }
        public DbSet<DalTask> Tasks { get; set; }
    }
}
