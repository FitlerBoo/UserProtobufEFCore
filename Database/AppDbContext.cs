using Microsoft.EntityFrameworkCore;
using UserProtobufEFCore.Database.Configurations;
using UserProtobufEFCore.Database.Models;

namespace UserProtobufEFCore.Database
{
    class AppDbContext(DbContextOptions<AppDbContext> option)
        : DbContext (option)
    {
        DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
