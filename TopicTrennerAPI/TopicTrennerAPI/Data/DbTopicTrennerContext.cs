using Microsoft.EntityFrameworkCore;
using TopicTrennerAPI.Models;


namespace TopicTrennerAPI.Data
{
    public class DbTopicTrennerContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("Server=127.0.0.1; Port=3306; Database=smile; user=root; Pwd=lfasdFl3sa6hRdlk5hVd3lkHjsa; charset=utf8;");
        }

        public DbSet<SimpleRule> Rules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SimpleRule>().ToTable("Rule");
        }
    }
}
