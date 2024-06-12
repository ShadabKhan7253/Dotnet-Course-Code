using API_Start.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Start.Data
{
    public class DataContextEF : DbContext  
    {
        private readonly IConfiguration _config;

        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }

        // DbSet is use to map the model back to the table for each model we wamt to acccess database
        public virtual DbSet<User> Users { get;set;}

        public virtual DbSet<UserSalary> UserSalary { get;set;}
        public virtual DbSet<UserJobInfo> UserJobInfo { get;set;}

        // Here we get the connectionstring

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }

        // Entity Framework on its own is going to look at the DBO schema in our database to try to find the table i.e User,UserSalary,UserJobInfo but it can't be there Becuase we have to inside the database inside our schema TutorialAppSchema so if entiry framwork goin to look for the table it cannot find them DBO.User DBO.UserSalary DBO.UserJobInfo it is there in TutorialAppSchema.User and so on 

        // so we can tell the entity framwork where the table is avtually there 

        // it will override the initial onModelCreating that exit in DBContext which we have inherited 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // here we will setup our logic to map our schema to access the correct schema i,e TutorialAppSchema in our example
            modelBuilder.HasDefaultSchema("TutorialAppSchema"); 
            // we tell our modelBuilder that our entity User is actually have a different table i.e Users
            // we also set our primary key here so that entity framework know that when we insert a new row we dont this field it will automatically populated for us it cant be repeated

            modelBuilder.Entity<User>()
                .ToTable("Users","TutorialAppSchema")
                .HasKey(u => u.UserId);

            modelBuilder.Entity<UserSalary>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<UserJobInfo>()
                .HasKey(u => u.UserId);
        }

    }
}