using Microsoft.EntityFrameworkCore;
using TaskApi.Models;

namespace TaskApi.Helpers
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<TaskType> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<User>().ToTable("users").HasKey(l => l.user_id);
            modelBuilder.Entity<List>().ToTable("lists").HasKey(l => l.list_id);
            modelBuilder.Entity<TaskType>().ToTable("tasks").HasKey(l => l.task_id);

            //sql default statement for creation time werent working
            //this code block makes ef ignore task_created duing the insertion
            modelBuilder.Entity<TaskType>()
            .Property(t => t.task_created) //this one selects the specific property
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'") //gives ef the default statement of db, its not necessary but might be helpful in the future
            .ValueGeneratedOnAdd(); //ef now knows value will be generated so it doesnt submit the value

            modelBuilder.Entity<List>()
            .Property(l => l.list_created)
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
            .Property(l => l.user_created)
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
            .ValueGeneratedOnAdd();

        }
    }
}