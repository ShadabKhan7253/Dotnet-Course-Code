using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Database.Data
{
    public class DataContextEF : DbContext
    {
        private IConfiguration _config;

        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }
        public DbSet<Computer>? Computer { get; set; } 
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                // options.UseSqlServer("Server=localhost;Database=DotNetCourseDatabase;Trusted_connection=false;TrustServerCertificate=True;User Id=sa;Password=SQLConnect1;",
                options.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                    options => options.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");

            modelBuilder.Entity<Computer>()
                // .HasNoKey()
                .HasKey(c => c.ComputerId);
                // .ToTable("Computer", "TutorialAppSchema");
                // .ToTable("TableName", "SchemaName");
        }
        

    }
}