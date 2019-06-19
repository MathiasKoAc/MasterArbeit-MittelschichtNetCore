using Microsoft.EntityFrameworkCore;
using System;
using TopicTrennerAPI.Models;


namespace TopicTrennerAPI.Data
{
    public class DbTopicTrennerContext : DbContext
    {
        public DbTopicTrennerContext(DbContextOptions<DbTopicTrennerContext> options) : base(options)
        {
        }

        public DbSet<SimpleRule> Rules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SimpleRule>().ToTable("Rule");
        }
    }
}
